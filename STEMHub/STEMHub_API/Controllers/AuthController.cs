using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Service.Authentication.Login;
using STEMHub.STEMHub_Service.Authentication.SignUp;
using STEMHub.STEMHub_Service.Authentication.User;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.Interfaces;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUserManagement _user;
        

        public AuthController(UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IUserManagement user,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _user = user;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            var tokenResponse = await _user.CreateUserWithTokenAsync(registerUser);
            if (tokenResponse.IsSuccess && tokenResponse.Response != null)
            {
                await _user.AssignRoleToUserAsync(registerUser.Roles, tokenResponse.Response.User);

                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { tokenResponse.Response.Token, email = registerUser.Email }, Request.Scheme);

                var message = new Message(new string[] { registerUser.Email! }, "STEMHub", $"<a href=\"{confirmationLink}\">Xác nhận tại đây</a>");
                var responseMsg = _emailService.SendEmail(message);
                return StatusCode(StatusCodes.Status200OK,
                    new Response { IsSuccess = true, Message = $"{tokenResponse.Message} {responseMsg}" });

            }

            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Message = tokenResponse.Message, IsSuccess = false });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Thành công", Message = "Xác minh thành công!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Người dùng không tồn tại!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            var loginOtpResponse = await _user.GetOtpByLoginAsync(loginModel);
            if (loginOtpResponse.Response != null)
            {
                var user = loginOtpResponse.Response.User;
                if (user.TwoFactorEnabled)
                {
                    var token = loginOtpResponse.Response.Token;
                    var message = new Message(new string[] { user.Email! }, "XÁC NHẬN OTP", token);
                    _emailService.SendEmail(message);

                    return StatusCode(StatusCodes.Status200OK,
                        new Response { IsSuccess = loginOtpResponse.IsSuccess, Status = "Thành công", Message = $"Chúng tôi đã gửi OPT đến {user.Email}. Vui lòng kiểm tra!" });
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var serviceResponse = await _user.GetJwtTokenAsync(user);
                    return Ok(serviceResponse);

                }
            }
            return Unauthorized();

        }

        [HttpPost]
        [Route("login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string userName)
        {
            var jwt = await _user.LoginUserWithJWTokenAsync(code, userName);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Thành công", Message = $"Mã không hợp lệ" });
        }

        [HttpPost]
        [Route("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(LoginResponse tokens)
        {
            var jwt = await _user.RenewAccessTokenAsync(tokens);
            if (jwt.IsSuccess)
            {
                return Ok(jwt);
            }
            return StatusCode(StatusCodes.Status404NotFound,
                new Response { Status = "Thành công", Message = $"Mã không hợp lệ" });
        }

    }
}

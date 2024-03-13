using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Service;
using STEMHub.STEMHub_Service.Authentication.Login;
using STEMHub.STEMHub_Service.Authentication.SignUp;
using STEMHub.STEMHub_Service.Authentication.TwoFactor;
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
            var userEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginModel.Username);
            if (userEmail == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response {Status = "Thất bại", Message = $"Người dùng {loginModel.Username} không tồn tại." });
            }

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(userEmail, userEmail.PasswordHash, loginModel.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new Response { Status = "Thất bại", Message = $"Mật khẩu không khớp!" });
            }

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

        #region 2FA

        [HttpPost("enable-2fa")]
        public async Task<IActionResult> EnableTwoFactorAuthentication(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.TwoFactorEnabled == false)
            {
                var twoFactorResponse = await _user.GenerateEnableTwoFactorOTPAsync(user);
                if (twoFactorResponse.Response != null)
                {
                    var token = twoFactorResponse.Response.Token;
                    await _userManager.UpdateAsync(user);

                    var message = new Message(new string[] { user.Email! }, "XÁC MINH 2 BƯỚC", $"Mã OTP của bạn là: {token}");
                    _emailService.SendEmail(message);
                }
            }
            else
            {
                return BadRequest(new { Message = "Yêu cầu không thành công! Tài khoản của bạn đã bật xác thực hai yếu tố" });
            }
            
            return Ok(new { Message = $"Bạn đang yêu cầu được bật 2 yếu tố xác thực. Vui lòng kiểm tra email: {user.Email} của bạn để nhận mã OTP." });
        }

        [HttpPost("disable-2fa")]
        public async Task<IActionResult> DisableTwoFactorAuthentication(string userId, bool enable)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.TwoFactorEnabled == true)
            {
                var twoFactorResponse = await _user.GenerateEnableTwoFactorOTPAsync(user);
                if (twoFactorResponse.Response != null)
                {
                    var token = twoFactorResponse.Response.Token;
                    await _userManager.UpdateAsync(user);
                    var message = new Message(new string[] { user.Email! }, "XÁC MINH 2 BƯỚC", $"Mã OTP của bạn là: {token}");
                    _emailService.SendEmail(message);
                }
            }
            else
            {
                return BadRequest(new { Message = "Yêu cầu không thành công! Tài khoản của bạn chưa được bật xác thực hai yếu tố" });
            }
            return Ok(new { Message = $"Bạn đang yêu cầu được tắt 2 yếu tố xác thực. Vui lòng kiểm tra email: {user.Email} của bạn để nhận mã OTP." });
        }


        [HttpPost("confirm-2fa")]
        public async Task<IActionResult> ConfirmTwoFactorAuthentication(string userId, string otpCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Xác nhận mã OTP
            if (user.OTPEnableTwoFactor == otpCode)
            {
                user.TwoFactorEnabled = !user.TwoFactorEnabled;

                user.OTPEnableTwoFactor = null;
                await _userManager.UpdateAsync(user);

                return Ok(new { Message = "Xác thực hai yếu tố đã được cập nhật." });
            }
            else
            {
                return BadRequest(new { Message = "Mã OTP không hợp lệ." });
            }
        }



        #endregion
    }
}

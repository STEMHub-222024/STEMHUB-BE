using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Data.DTO;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Services.Authentication.Login;
using STEMHub.STEMHub_Services.Constants;
using STEMHub.STEMHub_Services.Interfaces;
using System.Web;

namespace STEMHub.STEMHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {

        private readonly IUserManagement _user;
        private readonly STEMHubDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public UserController(UnitOfWork unitOfWork, 
            IUserManagement user, 
            STEMHubDbContext context,
            IEmailService emailService,
            UserManager<ApplicationUser> userManager) : base(unitOfWork)
        {
            _user = user;
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
        }


        [HttpGet]
        public  async Task<IActionResult> GetAllUser()
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetAllAsync<ApplicationUser>();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdUser(string id)
        {
            var user = await _unitOfWork.ApplicationUserRepository_UD.GetByIdUserAsync<UserDto>(id);

            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

            return Ok(user);
        }
        

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDto updatedUserModel)
        {
            try
            {
                var existingUserEntity = await _unitOfWork.ApplicationUserRepository_UD.GetByIdUserAsync<ApplicationUser>(id);
                if (existingUserEntity == null)
                    return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Thất bại", Message = "ID không tồn tại. Vui lòng kiểm tra lại" });

                existingUserEntity.LastName = updatedUserModel.LastName;
                existingUserEntity.FirstName = updatedUserModel.FirstName;
                existingUserEntity.Image = updatedUserModel.Image;
                existingUserEntity.Email = updatedUserModel.Email;
                existingUserEntity.PhoneNumber = updatedUserModel.PhoneNumber;

                await _unitOfWork.ApplicationUserRepository_UD.UpdateUserAsync(existingUserEntity);

                await _unitOfWork.CommitAsync();

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (Exception e)
            {
                //if (_uniqueConstraintHandler.IsUniqueConstraintViolation(e))
                //{
                //    Log.Error(e, "Vi phạm trùng lặp!");
                //    return BadRequest(new { ErrorMessage = "Vi phạm trùng lặp!", ErrorCode = "DUPLICATE_KEY" });
                //}
                //else
                //{
                return StatusCode(500, "Internal Server Error");
                //}
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userEntity = await _unitOfWork.ApplicationUserRepository_UD.GetByIdUserAsync<IdentityUser>(id);

            if (userEntity == null)
                return NotFound();

            await _unitOfWork.ApplicationUserRepository_UD.DeleteUserAsync(id);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
            await _userManager.UpdateAsync(user);

            var resetUrl = $"http://localhost:3000/resetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(model.Email)}";
            var emailContent = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333333; max-width: 600px; margin: 0 auto;'>
                        <p>Xin chào,</p>
                        <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản STEM của bạn. Vui lòng nhấp vào nút bên dưới để tiếp tục quá trình đặt lại mật khẩu:</p>
                        <p style='text-align: center;'>
                            <a href='{resetUrl}' style='background-color: #007bff; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer; border-radius: 5px;'>Đặt lại mật khẩu</a>
                        </p>
                        <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                        <p>Trân trọng,<br>Đội ngũ STEM</p>
                    </body>
                </html>";

            var message = new Message(new string[] { user.Email! }, "Đặt lại mật khẩu", emailContent);
            _emailService.SendEmail(message);

            return Ok($"Link đặt lại mật khẩu đã được gửi tới {model.Email} thành công! Vui lòng kiểm tra lại email.");

        }

        [HttpPost]
        [Route("api/resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng với email đã cung cấp.");
            }


            // Xác minh token
            //var isValidToken = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", decodedToken);
            var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", model.Token);

            if (!isValidToken)
            {
                return BadRequest("Token không hợp lệ.");
            }

            // Thực hiện đặt lại mật khẩu
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!resetPasswordResult.Succeeded)
            {
                return BadRequest("Lỗi khi đặt lại mật khẩu.");
            }

            return Ok("Mật khẩu đã được đặt lại thành công.");
        }

    }
}

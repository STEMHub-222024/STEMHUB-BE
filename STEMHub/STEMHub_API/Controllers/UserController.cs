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
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
            await _userManager.UpdateAsync(user);

            var resetUrl = $"http://localhost:3000/forgotPassword?token={Uri.EscapeDataString(token)}";
            var emailContent = $"<p>Vui lòng <a href='{resetUrl}'>nhấp vào đây</a> để đặt lại mật khẩu của bạn.</p>";

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

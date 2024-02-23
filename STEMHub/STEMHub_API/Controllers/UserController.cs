using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Service;
using STEMHub.STEMHub_Service.Authentication.Login;
using STEMHub.STEMHub_Service.Constants;
using STEMHub.STEMHub_Service.DTO;
using STEMHub.STEMHub_Service.Interfaces;

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
            var user = await _unitOfWork.ApplicationUserRepository.GetAllAsync<IdentityUser>();
            return Ok(user);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng.");
            }

            var token = await _user.GeneratePasswordResetTokenAsync(user);
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.Now.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            var message = new Message(new string[] { user.Email! }, "Đặt lại mật khẩu OTP", $"{token}");
            _emailService.SendEmail(message);

            return Ok($"OTP được gửi tới {model.Email} thành công! Vui lòng kiểm tra lại email.");

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("Không tìm thấy người dùng.");
            }

            if (user.PasswordResetToken != request.Token || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Token không hợp lệ hoặc đã hết hạn.");
            }

            //var verifyTokenResult = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Token);
            //if (!verifyTokenResult)
            //{
            //    return BadRequest("Mã OTP không hợp lệ.");
            //}

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (resetPasswordResult.Succeeded)
            {
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;
                await _userManager.UpdateAsync(user);

                return Ok("Đổi mật khẩu thành công.");
            }
            else
            {
                var errors = string.Join(", ", resetPasswordResult.Errors.Select(e => e.Description));
                return BadRequest($"Đổi mật khẩu thất bại: {errors}");
            }
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
    }
}

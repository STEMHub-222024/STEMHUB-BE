using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Services.Authentication.Login;
using STEMHub.STEMHub_Services.Authentication.SignUp;
using STEMHub.STEMHub_Services.Authentication.TwoFactor;
using STEMHub.STEMHub_Services.Authentication.User;

namespace STEMHub.STEMHub_Services.Interfaces
{
    public interface IUserManagement
    {
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUser registerUser);
        Task<ApiResponse<List<string>>> AssignRoleToUserAsync(List<string> roles, ApplicationUser user);
        Task<ApiResponse<LoginOtpResponse>> GetOtpByLoginAsync(Login loginModel);
        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(ApplicationUser user);
        Task<ApiResponse<LoginResponse>> LoginUserWithJWTokenAsync(string otp, string userName);
        Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens);
        Task<ApiResponse<LoginResponse>> ResetPasswordAndGetJwtTokenAsync(ApplicationUser user, string newPassword);
        Task<ApiResponse<TwoFactorResponse>> GenerateDisableTwoFactorOTPAsync(ApplicationUser user);
        Task<ApiResponse<TwoFactorResponse>> GenerateEnableTwoFactorOTPAsync(ApplicationUser user);
    }
}

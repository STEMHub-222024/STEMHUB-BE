using STEMHub.STEMHub_Data.Data;

namespace STEMHub.STEMHub_Service.Authentication.User
{
    public class CreateUserResponse
    {
        public string Token { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}

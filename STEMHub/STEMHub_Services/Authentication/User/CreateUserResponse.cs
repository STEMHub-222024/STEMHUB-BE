using STEMHub.STEMHub_Data.Data;

namespace STEMHub.STEMHub_Services.Authentication.User
{
    public class CreateUserResponse
    {
        public string Token { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}

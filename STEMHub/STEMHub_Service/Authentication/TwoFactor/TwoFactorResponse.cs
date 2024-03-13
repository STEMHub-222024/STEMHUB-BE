using STEMHub.STEMHub_Data.Data;

namespace STEMHub.STEMHub_Service.Authentication.TwoFactor
{
    public class TwoFactorResponse
    {
        public string Token { get; set; } = null!;
        public bool IsTwoFactorEnable { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}

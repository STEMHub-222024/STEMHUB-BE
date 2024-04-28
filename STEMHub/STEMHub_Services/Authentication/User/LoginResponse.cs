namespace STEMHub.STEMHub_Services.Authentication.User
{
    public class LoginResponse
    {
        public TokenType? AccessToken { get; set; }
        public TokenType? RefreshToken { get; set; }
    }
}

﻿namespace STEMHub.STEMHub_Service.Authentication.User
{
    public class TokenType
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiryTokenDate { get; set; }
    }
}

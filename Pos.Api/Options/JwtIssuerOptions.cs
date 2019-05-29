using System;
using Microsoft.IdentityModel.Tokens;

namespace Pos.Api.Options
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime NotBefore => DateTime.Now;
        public DateTime IssuedAt => DateTime.Now;
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(30);
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public SigningCredentials SigningCredentials { get; set; }
    }
}

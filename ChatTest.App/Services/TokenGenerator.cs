using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ChatTest.App.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private const string _app = "ChatTest.App";

        public string Generate(string user, string client)
        {
            byte[] privateKeyRaw = Convert.FromBase64String(Constants.PrivateKey);

            // creating the RSA key 
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportPkcs8PrivateKey(new ReadOnlySpan<byte>(privateKeyRaw), out _);
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(provider);

            // Generating the token 
            var now = DateTime.UtcNow;

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, client),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user)
                };

            var handler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken
            (
                _app,
                client,
                claims,
                now.AddMilliseconds(-30),
                now.AddDays(31),
                new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            );

            return handler.WriteToken(token);
        }
    }
}

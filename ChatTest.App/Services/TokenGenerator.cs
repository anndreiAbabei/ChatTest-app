using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ChatTest.App.Models;

namespace ChatTest.App.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private const string App = "ChatTest.App";

        public string Generate(string user, CliendId client)
        {
            byte[] privateKeyRaw = Convert.FromBase64String(Constants.PrivateKey);

            // creating the RSA key 
            var provider = new RSACryptoServiceProvider();
            provider.ImportPkcs8PrivateKey(new ReadOnlySpan<byte>(privateKeyRaw), out _);
            var rsaSecurityKey = new RsaSecurityKey(provider);

            // Generating the token 
            DateTime now = DateTime.UtcNow;

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, client.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user),
                    new Claim(JwtRegisteredClaimNames.NameId, client.ConnectionId), 
                };

            var handler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken
            (
                App,
                client.Id,
                claims,
                now.AddMilliseconds(-30),
                now.AddDays(31),
                new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            );

            return handler.WriteToken(token);
        }



        public string GetName(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            return new JwtSecurityTokenHandler()
                  .ReadJwtToken(token)?
                  .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?
                  .Value;
        }

        public string GetConnectionId(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            return new JwtSecurityTokenHandler()
                  .ReadJwtToken(token)?
                  .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?
                  .Value;
        }



        public bool IsValid(string token)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(Constants.PrivateKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                                           {
                                               ValidateLifetime = false, // Because there is no expiration in the generated token
                                               ValidateAudience = false, // Because there is no audiance in the generated token
                                               ValidateIssuer = true,   // Because there is no issuer in the generated token
                                               ValidIssuer = App,
                                               ValidAudience = App,
                                               IssuerSigningKey = new SymmetricSecurityKey(key)
                                           };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return true; //todo change
            }
        }
    }
}

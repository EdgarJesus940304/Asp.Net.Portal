using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Handler
{
    public class LoginHandler
    {
        public SecurityToken CreateToken(string userId, string userName, string userProfileId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            using (var cryptoProvider = new RSACryptoServiceProvider(2048))
            {
                var rsaKey = cryptoProvider.ExportParameters(true);


                ClaimsIdentity identity = new ClaimsIdentity(
                    new[] {
                    new Claim("ID", userId),
                    new Claim("USERNAME", userName),
                    }
                );
                return tokenHandler.CreateToken(new SecurityTokenDescriptor
                {
                    SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsaKey), SecurityAlgorithms.RsaSha256Signature),
                    Subject = identity,
                    Expires = DateTime.Now.AddHours(12)
                });
            }
        }

        public string CreateStringToken(string userId, string userName, string userProfileId)
        {
            var token = CreateToken(userId, userName, userProfileId);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

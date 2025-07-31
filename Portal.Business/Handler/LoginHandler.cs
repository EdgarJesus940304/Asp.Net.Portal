using Microsoft.IdentityModel.Tokens;
using Portal.Business.Models;
using Portal.Business.Utils;
using Portal.Business.WebService;
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
        public async Task<MessageResponse<UserModel>> Login(UserModel userModel)
        {
            try
            {
                var service = new ApiBaseService<UserModel>(ServiceParameters.ENDPOINT_USERS);

                return await service.Login<UserModel>(userModel);
            }
            catch (Exception ex)
            {
                return new MessageResponse<UserModel>()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }
        }

        public SecurityToken CreateToken(string userId, string userName)
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

        public string CreateStringToken(string userId, string userName)
        {
            var token = CreateToken(userId, userName);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using Portal.Business.Models;
using Portal.Business.Utils;
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
        public MessageResponse Login(UserModel userModel)
        {
            try
            {
                string userKey = userModel.UserName;
                string userPassword = userModel.Password;
                //var userFind = db.Users.FirstOrDefault(x => x.UserKey.ToUpper() == userKey.ToUpper());

                //if (userFind is null)
                //{
                //    return new MessageResponse()
                //    {
                //        Number = 404,
                //        Message = "El usuario capturado no existe".ToUpper()
                //    };
                //}


                return new MessageResponse()
                {
                    Number = 200,
                    Message = "Usuario correcto".ToUpper(),
                    //Data = new UserModel()
                    //{
                    //    Id = userFind.Id,
                    //    UserKey = userFind.UserKey,
                    //    UserName = userFind.UserName,
                    //    UserPassword = userFind.UserPassword,
                    //    UserProfileId = userFind.UserProfileId ?? 0
                    //}
                };

                //if (userPassword == Utilidades.DesEncriptar(userFind.UserPassword))
                //{
                    
                //}
                //else
                //{
                //    return new MessageResponse()
                //    {
                //        Number = 404,
                //        Message = "La contraseña capturada es incorrecta".ToUpper()
                //    };
                //}

            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    Message = ex.Message,
                    Number = 500
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

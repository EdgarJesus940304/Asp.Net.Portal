using Portal.Business.Models;
using Portal.Business.Utils;
using Portal.Business.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Handler
{
    public class UserHandler
    {
        public async Task<MessageResponse> SaveUser(UserModel user)
        {
            try
            {
                var service = new BaseService<UserModel>(EndPoints.ENDPOINT_USERS);

                return await service.Post(user);
            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"{ex.Message} {ex?.InnerException?.Message}"
                };
            }

        }

    }
}

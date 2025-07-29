using Portal.Business.Models;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Business.Handler
{
    public class UserHandler
    {
        public MessageResponse GetUsers()
        {
            try
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.OK,
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new MessageResponse()
                {
                    ResponseType = ResponseType.Error,
                    Message = $"No fue posible obtener el usuario especificado {ex.Message} {ex?.InnerException?.Message}"
                };
            }

        }
    }
}

using Portal.Business.Models;
using Portal.Business.Models.DataTables;
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
        public async Task<RootResult<UserModel>> GetUserListAsync(FilterDataTableModel model)
        {
            try
            {
                var service = new ApiBaseService<FilterDataTableModel>(ServiceParameters.ENDPOINT_USERS);

                return await service.List<UserModel>(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public async Task<MessageResponse<UserModel>> GetUserAsync(int id)
        {
            try
            {
                var service = new ApiBaseService<UserModel>(ServiceParameters.ENDPOINT_USERS);

                return await service.Get<UserModel>(id);
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
        public async Task<MessageResponse> CreateUserAsync(UserModel user)
        {
            try
            {
                var service = new ApiBaseService<UserModel>(ServiceParameters.ENDPOINT_USERS);

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
        public async Task<MessageResponse> ModifyUserAsync(UserModel user)
        {
            try
            {
                var service = new ApiBaseService<UserModel>(ServiceParameters.ENDPOINT_USERS);

                return await service.Put(user.Id, user);
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
        public async Task<MessageResponse> RemoveUserAsync(int userId)
        {
            try
            {
                var service = new ApiBaseService<UserModel>(ServiceParameters.ENDPOINT_USERS);

                return await service.Delete(userId);
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

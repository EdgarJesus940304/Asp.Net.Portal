using Portal.Business.Handler;
using Portal.Business.Models;
using Portal.Business.Models.DataTables;
using Portal.Business.Utils;
using Portal.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Portal.UI.Controllers
{
    [LoginValidate]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListUsers(FilterDataTableModel model)
        {
            UserHandler userHandler = new UserHandler();

            var response = await userHandler.ListUsers(model);

            return Json(response.Data);


        }

        public async Task<ActionResult> GetUser(int Id)
        {
            UserHandler userHandler = new UserHandler();

            var response = await userHandler.GetUser(Id);
            if (response.ResponseType == ResponseType.OK)
            {
                return Json(new MessageResponse<UserModel>()
                {
                    Message = response.Message,
                    Number = 200,
                    Data = response.Data
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 500
                }, JsonRequestBehavior.AllowGet);
            }

        }


        public async Task<ActionResult> SaveUser(UserModel user)
        {
            UserHandler userHandler = new UserHandler();

            var response = await userHandler.SaveUser(user);
            if (response.ResponseType == ResponseType.OK)
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 200
                });
            }
            else
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 500
                });
            }

        }

        public async Task<ActionResult> UpdateUser(UserModel user)
        {
            UserHandler userHandler = new UserHandler();

            var response = await userHandler.UpdateUser(user);
            if (response.ResponseType == ResponseType.OK)
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 200
                });
            }
            else
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 500
                });
            }

        }

        public async Task<ActionResult> DeleteUser(int userId)
        {
            UserHandler userHandler = new UserHandler();

            var response = await userHandler.DeleteUser(userId);
            if (response.ResponseType == ResponseType.OK)
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 200
                });
            }
            else
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 500
                });
            }

        }
    }
}
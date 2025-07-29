using Portal.Business.Handler;
using Portal.Business.Models;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Portal.UI.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
    }
}
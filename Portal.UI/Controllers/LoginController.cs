using Portal.Business.Handler;
using Portal.Business.Models;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Portal.UI.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> Login(UserModel userModel)
        {

            LoginHandler handler = new LoginHandler();
            MessageResponse<UserModel> response = await handler.Login(userModel);
            if (response.ResponseType == ResponseType.OK)
            {
                UserModel LoggedUser = response.Data;
                DateTime requestAt = DateTime.Now;
                DateTime expiresIn = DateTime.Now.AddDays(2);

                var token = handler.CreateStringToken(LoggedUser?.Id.ToString(), LoggedUser.UserName);

                HttpCookie redirectUrlCookie = Request.Cookies.Get("returnUrl");
                HttpCookie cookie = new HttpCookie("Cookie_Session")
                {
                    Value = token,
                    Expires = DateTime.Now.AddDays(30)
                };
                Response.Cookies.Add(cookie);

                return Json(new MessageResponse<UserModel>()
                {
                    Message = response.Message,
                    Number = 200,
                    Data = response.Data
                });
            }
            else if (response.ResponseType == ResponseType.Warning)
            {
                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 400
                });
            }
            else
            {
                Response.ClearHeaders();
                Response.ClearContent();
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new MessageResponse()
                {
                    Message = response.Message,
                    Number = 500
                });
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get("returnUrl");

                if (!string.IsNullOrWhiteSpace(cookie?.Value))
                {
                    cookie.Expires = DateTime.Now.AddSeconds(1);
                    HttpContext.Request.Cookies.Add(cookie);
                }

                MessageResponse response = new MessageResponse();
                response.Number = 200;
                response.Message = "OK";
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new MessageResponse()
                {
                    Number = 500,
                    Message = "No fue posible cerrar sesión " + ex.Message

                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
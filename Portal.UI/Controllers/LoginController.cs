using Portal.Business.Models;
using Portal.Business.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private string GenerateToken(UserModel user, DateTime expires)
        {
            try
            {
                Bussiness.Handlers.SessionHandler sessionHandler = new Bussiness.Handlers.SessionHandler();
                return sessionHandler.CreateStringToken(user?.Id.ToString(), user?.UserName, (user?.UserProfileId)?.ToString());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public ActionResult Login(UserModel userModel)
        {
            try
            {
                using (var handler = new LoginHandler())
                {
                    UserModel LoggedUser = new UserModel();
                    var response = handler.Login(userModel);
                    if (response.Number == 200)
                    {
                        LoggedUser = (UserModel)response.Data;
                        DateTime requestAt = DateTime.Now;
                        DateTime expiresIn = DateTime.Now.AddDays(2);
                        var token = GenerateToken(LoggedUser, expiresIn);

                        HttpCookie redirectUrlCookie = Request.Cookies.Get("returnUrl");
                        HttpCookie cookie = new HttpCookie("Cookie_Session")
                        {
                            Value = token,
                            Expires = DateTime.Now.AddDays(30)
                        };
                        Response.Cookies.Add(cookie);
                    }

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new MessageResponse()
                {
                    Number = 500,
                    Message = "No fue posible obtener conexión para iniciar sesion, intente mas tarde " + ex.Message

                }, JsonRequestBehavior.AllowGet);
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
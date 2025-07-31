using Portal.Business.Models;
using Portal.UI.Utils.CustomActionResults;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.UI.Utils
{
    public class LoginValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies.Get("Cookie_Session");
            //HttpCookie cookie2 = filterContext.HttpContext.Request.Cookies.Get("returnUrl");


            if (string.IsNullOrWhiteSpace(cookie?.Value))
            {
                if (filterContext.HttpContext.Request.Url.PathAndQuery.Contains("id"))
                {
                    //filterContext.Controller.ViewBag.RequestedUrl = filterContext.HttpContext.Request.RawUrl;
                    filterContext.HttpContext.Response.Cookies.Add(new HttpCookie("returnUrl", filterContext.HttpContext.Request.Url.PathAndQuery)
                    {
                        Expires = DateTime.Now.AddMinutes(3)
                    });
                    filterContext.Result = new RedirectToLoginActionResult();
                }
                else
                {
                    filterContext.Result = new RedirectToLoginActionResult();
                }
            }
            else
            {
                var token = handler.ReadJwtToken(cookie?.Value);
                string IdUser = token.Payload["ID"].ToString();
                string UserName = token.Payload["USERNAME"].ToString();

                filterContext.Controller.ViewBag.LoggedUser = new UserModel()
                {
                    Id = Convert.ToInt32(IdUser),
                    Name = UserName
                };
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.UI.Utils.CustomActionResults
{
    public class RedirectToLoginActionResult : RedirectToRouteResult
    {
        public RedirectToLoginActionResult() : base(new RouteValueDictionary(new
        {
            controller = "Login",
            action = "Index"
        }))
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            base.ExecuteResult(context);
        }
    }
}
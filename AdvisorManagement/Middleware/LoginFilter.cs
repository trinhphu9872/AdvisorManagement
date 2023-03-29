using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Middleware
{
    public class LoginFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["EmailVLU"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
}
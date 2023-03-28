using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Middleware
{

    public class RoleMiddleware : Controller
    {
        public ActionResult AutoRedirect(string userMail)
        {
            return RedirectToAction("Index","Home");
        }
    }
}
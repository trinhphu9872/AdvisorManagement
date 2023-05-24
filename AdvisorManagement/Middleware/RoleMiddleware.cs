using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvisorManagement.Middleware
{

    public class RoleMiddleware : Controller
    {
        AccountMiddleware serviceAccount = new AccountMiddleware();
        public ActionResult AutoRedirect(string userMail)
        {
            if (int.Parse(serviceAccount.getRoleTextName(userMail)) ==  3 )
            {
                return RedirectToAction("UserProfile", "Home", new { id = "", email = userMail, area = "" });
            }
            return RedirectToAction("Index","Home");
        }
    }
}
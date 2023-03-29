using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using AdvisorManagement.Models;
using AdvisorManagement.Middleware;
using System.Data.Entity;
using System.Security.Claims;

namespace AdvisorManagement.Controllers
{
  
    public class AccountController : Controller
    {

        private CP25Team09Entities dbApp = new CP25Team09Entities();
        private RoleMiddleware roleChange = new RoleMiddleware();
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();

        public void init()
        {
            Session["Name"] = accountService.getTextName(User.Identity.Name);
            Session["RoleName"] = accountService.getRoleTextName(User.Identity.Name);
        }
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return roleChange.AutoRedirect(User.Identity.Name);
            }
            return View();
        }
        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = Url.Action("SignInCallback") },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("~/Home/Index");
        }

        public ActionResult SignInCallback()
        {
            Session["EmailVLU"] = User.Identity.Name;
 
            var query = dbApp.AccountUser.Where(x => x.email == User.Identity.Name).ToList();
            if (query.Count() == 0)
            {
                accountService.UserProfile((ClaimsIdentity)User.Identity);
                this.init();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                if (query.Count() > 0)
                {
                    this.init();
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    Session["AccountBlocked"] = true;
                    return RedirectToAction("Login", "Account");
                }
            }
        }
    }
}

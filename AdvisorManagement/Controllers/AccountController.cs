using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;

namespace AdvisorManagement.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            //if (!Request.IsAuthenticated)
            //{
            //    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
            //        OpenIdConnectAuthenticationDefaults.AuthenticationType);
            //}
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = Url.Action("HomeRedirect") },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        //public void SignOut()
        //{
        //    //string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

        //    //HttpContext.GetOwinContext().Authentication.SignOut(
        //    //    new AuthenticationProperties { RedirectUri = callbackUrl },
        //    //    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        //}

        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("~/Home/Index");
        }


        public ActionResult HomeRedirect()
        {
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult SignOutCallback()
        //{
        //    if (Request.IsAuthenticated)
        //    {
        //        // Redirect to home page if the user is authenticated.
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View();
        //}
    }
}

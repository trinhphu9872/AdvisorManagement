using AdvisorManagement.Models.ViewModel;
using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Middleware;
using System.Threading.Tasks;

namespace AdvisorManagement.Controllers
{

    public class HomeController : Controller
    {
        // Service and Database
        private CP25Team09Entities dbApp = new CP25Team09Entities();
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        public ActionResult Index()
        {
            string user_mail = User.Identity.Name;
            if (user_mail != null && user_mail != "")
            {
                var sql = dbApp.AccountUser.FirstOrDefault(x => x.email == user_mail);
                if (sql == null)
                {
                    accountService.UserProfile((ClaimsIdentity) User.Identity);
                }
                ViewBag.menu = serviceMenu.getMenu(user_mail);
            }
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
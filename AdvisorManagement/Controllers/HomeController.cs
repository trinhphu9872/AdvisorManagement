using AdvisorManagement.Models.ViewModel;
using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Middleware;

namespace AdvisorManagement.Controllers
{

    public class HomeController : Controller
    {
        private CP25Team09Entities dbApp = new CP25Team09Entities();
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        public ActionResult Index()
        {
            string SessionRe = "";
            int roles = 3;
            string user_mail = User.Identity.Name;
            if (user_mail != null && user_mail != "")
            {
                var sql = dbApp.AccountUser.FirstOrDefault(x => x.email == user_mail);
                if (sql == null)
                {
                    AccountUser userNew = new AccountUser();
                    userNew.id_Role = roles;
                    // get Claims
                    StudentClass student = (StudentClass)(accountService.UserProfile((ClaimsIdentity)User.Identity, roles));

                    userNew.ID = Guid.NewGuid();
                    userNew.email = user_mail;
                    userNew.createtime = DateTime.Now;
                    //userNew.user_code = student.user_code;
                    //userNew.username = student.user_name;

                    //userNew.user_code =1
                    dbApp.AccountUser.Add(userNew);
                    dbApp.SaveChanges();
                    SessionRe = user_mail;
                }
                else
                {
                    SessionRe = sql.email;
                }
                var seeMenu = serviceMenu.getMenu(user_mail);
                ViewBag.menu = seeMenu;
                return View(seeMenu);
            }
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
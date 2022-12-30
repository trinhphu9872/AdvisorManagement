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
using System.Data.Entity;
using System.IO;

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
                ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
                ViewBag.avatar = sql.img_profile;
            }
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);

            return View();
        }
        public ActionResult UserProfile(string email)
        {
            AccountUser user = dbApp.AccountUser.FirstOrDefault(u => u.email.Equals(email));
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = user.img_profile;
            return View(user);
        }
        public ActionResult EditUserProfile(int id)
        {
            AccountUser user = dbApp.AccountUser.Find(id);
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = user.img_profile;
            return View(user);
        }
        [HttpPost]
        public ActionResult EditUserProfile(AccountUser user)
        {
            AccountUser edituser = dbApp.AccountUser.Find(user.id);
            edituser.user_name = user.user_name;
            edituser.user_code = user.user_code;
            edituser.phone = user.phone;
            if (user.ImageUpload != null)
            {
                string filename = Path.GetFileNameWithoutExtension(user.ImageUpload.FileName).ToString();
                string extension = Path.GetExtension(user.ImageUpload.FileName);
                filename = filename + extension;
                edituser.img_profile = "~/Images/imageProfile/" + filename;
                user.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Images/imageProfile/"), filename));
            }
            dbApp.Entry(edituser).State = EntityState.Modified;
            dbApp.SaveChanges();
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            return RedirectToAction("UserProfile", "Home", edituser);
        }
    }
}
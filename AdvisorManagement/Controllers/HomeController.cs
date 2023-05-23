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
    [LoginFilter]
    public class HomeController : Controller
    {
        // Service and Database
        private CP25Team09Entities dbApp = new CP25Team09Entities();
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();

        public void init()
        {
            ViewBag.menu = serviceMenu.getMenu(User.Identity.Name);
            ViewBag.avatar = accountService.getAvatar(User.Identity.Name);
        }
        public ActionResult Index()
        {
            this.init();
            return View();
        }
        public ActionResult UserProfile(string email)
        {
            this.init();
            AccountUser user = dbApp.AccountUser.FirstOrDefault(u => u.email.Equals(User.Identity.Name));
            user.img_profile = (user.img_profile != null && user.img_profile != "" && user.img_profile != " ") ? user.img_profile.ToString().Replace("~", "") : "/Images/imageProfile/avata.png";
            return View(user);
        }
        public ActionResult EditUserProfile(int id)
        {
            this.init();
            AccountUser user = dbApp.AccountUser.Find(id);
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
            this.init();
            dbApp.Entry(edituser).State = EntityState.Modified;
            dbApp.SaveChanges();
            return RedirectToAction("UserProfile", "Home", edituser);
        }
    }
}
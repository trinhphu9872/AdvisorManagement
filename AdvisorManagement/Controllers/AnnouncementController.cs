using AdvisorManagement.Hubs;
using AdvisorManagement.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvisorManagement.Models;
using Microsoft.Ajax.Utilities;

namespace AdvisorManagement.Controllers
{
    [LoginFilter]
    public class AnnouncementController : Controller
    {
        private AccountMiddleware accountService = new AccountMiddleware();
        private MenuMiddleware serviceMenu = new MenuMiddleware();
        private CP25Team09Entities db = new CP25Team09Entities();
        private AnnouncementMiddleware serviceAnnoun = new AnnouncementMiddleware();
        // GET: LearnProcess
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

        [HttpPost]
        public JsonResult SendNotification(string userID, string title, string message)
        {            
            var id_account = db.AccountUser.FirstOrDefault(x=>x.email == userID).id;
            NotificationHub objNotifHub = new NotificationHub();
            Annoucement objNotif = new Annoucement();
            objNotif.title = title;
            objNotif.message = message;
            db.Annoucement.Add(objNotif);
            db.SaveChanges();
            Notification notif = new Notification();
            notif.id_notification = objNotif.id;
            notif.send_to = userID;
            notif.create_time = DateTime.Now;
            notif.is_read = false;
                
            db.Configuration.ProxyCreationEnabled = false;            
            db.Notification.Add(notif);
            db.SaveChanges();

            objNotifHub.SendNotification(userID);

            return Json(new {message = "Gửi thành công"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]        
        public JsonResult UpdateNotification(int? id_notify)
        {            
            var notify = db.Notification.Find(id_notify);
            notify.is_read = true;            
            db.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("AllNotification", "Announcement", new { area = "" }) });
        }

        public ActionResult AllNotification()
        {
            var listNotify = serviceAnnoun.LoadNotifyData(User.Identity.Name);
            Session["listNotify"] = listNotify;
            this.init();
            return View();
        }

    }
}
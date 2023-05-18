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
using AdvisorManagement.Models.ViewModel;
using System.IO;
using System.Runtime.Remoting.Contexts;
using Spire.Pdf.Exporting.XPS.Schema;

namespace AdvisorManagement.Controllers
{
    [LoginFilter]
    public class AnnouncementController : Controller
    {
        private AccountMiddleware accountService = new AccountMiddleware();
        private MailServicesMiddleware mailService = new MailServicesMiddleware();

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
            int obj = db.Notification.Where(x => x.send_to == User.Identity.Name).ToList().Count();
            
            if (obj > 1)
            {
                obj = db.Notification.FirstOrDefault(x => x.send_to == User.Identity.Name).id;
            }
            ViewBag.id_notify = obj;
            this.init();
            return View();
        }

        [HttpPost]
        public JsonResult SendNotification(string userID, string title, string message)
        {
            HttpFileCollectionBase files = Request.Files;           
            if (!mailService.IsValidEmail(userID)) {
            
                return Json(new { success = false, message = "Mail không hợp lệ " }, JsonRequestBehavior.AllowGet);
            }

            if (message == "" || title == "")
            {
                return Json(new { success = false, message = "Vui lòng điền đầy đủ trường dữ liệu" }, JsonRequestBehavior.AllowGet);
            }

            if (db.AccountUser.Where(x => x.email == userID).ToList().Count() > 0)
            {
                var path_file = serviceAnnoun.GetFileAttach(files);
                var id_account = db.AccountUser.FirstOrDefault(x => x.email == userID).id;
                NotificationHub objNotifHub = new NotificationHub();
                Annoucement objNotif = new Annoucement();
                objNotif.title = title;
                objNotif.message = message;
                objNotif.file_attach = path_file;                   
                db.Annoucement.Add(objNotif);
                db.SaveChanges();
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname;
                    // Checking for Internet Explorer      
                    if(Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;                        
                    }
                    string path = Server.MapPath("~/Attach/" + objNotif.id);
                    // Get the complete folder path and store the file inside it.      
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    fname = System.IO.Path.Combine(path, fname);
                    file.SaveAs(fname);
                }
                Notification notif = new Notification();
                notif.id_notification = objNotif.id;
                notif.send_to = userID;
                notif.create_time = DateTime.Now;
                notif.is_read = false;
                MailRequest mail = new MailRequest();
                mail.To = userID;
                mail.Message = message;
                mail.Subject = title;
                mailService.MailSendRequest(mail, this.LoadHtmlTemplate());
                db.Configuration.ProxyCreationEnabled = false;
                db.Notification.Add(notif);
                db.SaveChanges();

                objNotifHub.SendNotification(userID);

                return Json(new { success = true,message = "Gửi thành công" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Gửi không thành công" }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]        
        public JsonResult UpdateNotification(int? id_notify)
        {            
            var notify = db.Notification.Find(id_notify);
            notify.is_read = true;            
            db.SaveChanges();
            return Json(new { redirectToUrl = Url.Action("AllNotification", "Announcement", new { area = "", id_notify = (int)id_notify }) });
        }


        [HttpPost]
        public JsonResult ClearNotification()
        {
            var obj  =  db.Notification.Where(x =>  x.send_to == User.Identity.Name).ToList();
            if (obj != null && obj.Any())
            {
                db.Notification.RemoveRange(obj);

            }
            else
            {
                return Json(new { success = false, message = "Dữ liệu trống" });

            }

            db.SaveChanges();
            return Json(new { success = true, message = "Xoá thành công" });

        }

        [HttpGet]
        public JsonResult DetailNotification(int? id_notify)
        {
            NotificationHub objNotifHub = new NotificationHub();
            var notify = db.Notification.Find(id_notify);
            if (notify != null)
            {
                notify.is_read = true;

            }
            db.SaveChanges();
            var detailNoti = serviceAnnoun.LoadDetailNotify(User.Identity.Name,(int)id_notify);
            objNotifHub.GetNotification(User.Identity.Name);         
            return Json(new { notify = detailNoti, message = "Lấy thành công"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllNotification(int id_notify)
        {
            Session["id_notify"] = id_notify;            
            var listNotify = serviceAnnoun.LoadNotifyData(User.Identity.Name);
            Session["listNotify"] = listNotify;
            this.init();
            return View();
        }

        public string LoadHtmlTemplate()
        {
            // Load the HTML template from a file, database or other source
            // Here's an example of loading from a file:

            var templateFilePath = Server.MapPath("~/Views/Shared/MailTemplate.html");

            string htmlTemplate = "";

            using (StreamReader reader = new StreamReader(templateFilePath))
            {
                htmlTemplate = reader.ReadToEnd();
            }

            return htmlTemplate;


            //return "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body bgcolor=\"#f5f5f5\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" offset=\"0\" style=\"padding:70px 0 70px 0;\">\r\n    <table width=\"600\" height=\"auto\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#fdfdfd; border:1px solid #dcdcdc; border-radius:3px !important;\">\r\n        <tr>\r\n            <td width=\"600\" height=\"auto\" bgcolor=\"#000\" border=\"0\" style=\"padding:36px 48px; display:block; margin: 0px auto;\">\r\n                <h1 style=\"color:#ffffff; font-family:&quot; Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:30px; line-height:150%; font-weight:300; margin:0; text-align:left;\">[[Subject]]</h1>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" bgcolor=\"#fdfdfd\" border=\"0\" style=\"color:#737373; font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:14px; line-height:150%; text-align:left; padding:48px;\">\r\n                <p style=\"margin:0 0 16px;\">Xin chào giảng viên <b>[[Name]]</b></p>\r\n                <p style=\"margin:0 0 16px;\">[[Message]]</p>\r\n                <p style=\"margin:0 0 16px;\">Trân trọng,</p>\r\n\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" border=\"0\" style=\"padding:0 48px 48px 48px; color:#707070; font-family:Arial; font-size:12px; line-height:125%; text-align:center;\">\r\n                <b>KHOA CÔNG NGHỆ THÔNG TIN - TRƯỜNG ĐẠI HỌC VĂN LANG</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
        }

        [HttpPost]
        public JsonResult DeleteNotification(int? id_notify)
        {
            NotificationHub objNotifHub = new NotificationHub();
            var notify = db.Notification.Find(id_notify);
            if (notify != null)
            {
                db.Notification.Remove(notify);
            }
            db.SaveChanges();          
            objNotifHub.GetNotification(User.Identity.Name);
            return Json(new { success = true, message = "Xóa thành công", redirectToUrl = @Url.Action("AllNotification", "Announcement", new { area = "", id_notify = 0 })}, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadFileAttach(int? id, string file_name)
        {
            var id_announ = db.Notification.Find(id).id_notification;
            var filePath = Server.MapPath("~/Attach/" + id_announ + "/") + file_name;           
            return File(filePath, "application/force- download", System.IO.Path.GetFileName(filePath));
        }
    }
}
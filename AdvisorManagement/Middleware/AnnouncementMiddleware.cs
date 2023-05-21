using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using Microsoft.AspNetCore.Hosting.Server;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Middleware
{
    public class AnnouncementMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();

        //Danh sách thông báo
        public object LoadNotifyData(string userId)
        {
            var query = (from n in db.Notification
                         join a in db.Annoucement on n.id_notification equals a.id
                         where n.id_notification == a.id && n.send_to == userId
                         select new UserNotification
                         {
                             id = n.id,
                             userID = n.send_to,
                             userSend = db.AccountUser.FirstOrDefault(x => x.id == n.id_user).email,

                             message = a.message,
                             title = a.title,
                             date = n.create_time,                          
                             isRead = n.is_read
                         }).OrderByDescending(x => x.date)
                       .ToList();
            return query;
        }

        public object LoadDetailNotify(string userId, int id_notify)
        {
            var query = (from n in db.Notification
                         join a in db.Annoucement on n.id_notification equals a.id
                         where n.send_to == userId && n.id == id_notify
                         select new UserNotification
                         {
                             id = n.id,
                             userSend = db.AccountUser.FirstOrDefault(x => x.id == n.id_user).email,
                             userID = n.send_to,
                             message = a.message,
                             title = a.title,
                             date = n.create_time,
                             isRead = n.is_read,
                             file_attach = a.file_attach
                         }).OrderByDescending(x => x.date)
                       .ToList();
            return query;
        }

        public string GetFileAttach(HttpFileCollectionBase files)
        {
            string path_file = "";
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;               
                fname = file.FileName;
                if (path_file == "")
                {
                   path_file += fname;
                }
                else
                {
                   path_file += ";" + fname;
                }                
            }

            if(path_file == "")
            {
                return null;
            }
            return path_file;
        }

        public int getId(string mail)
        {
            int id = db.AccountUser.FirstOrDefault(x => x.email == mail).id;
            return id;
        }
    }
}
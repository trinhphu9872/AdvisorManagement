using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
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
                         where n.id_notification == a.id && n.send_to == userId && n.id_notification == id_notify
                         select new UserNotification
                         {
                             id = n.id,
                             userID = n.send_to,
                             message = a.message,
                             title = a.title,
                             date = n.create_time,
                             isRead = n.is_read
                         }).OrderByDescending(x => x.date)
                       .ToList();
            return query;
        }
    }
}
using AdvisorManagement.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvisorManagement.Models.ViewModel;
using System.Threading.Tasks;

namespace AdvisorManagement.Hubs
{

    public class NotificationHub : Hub
    {
        private static readonly ConcurrentDictionary<string, UserHubModels> Users =
            new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);
        private CP25Team09Entities db = new CP25Team09Entities();
      /*  public void Send(string message)
        {
            Clients.All.notify(message);
        }
     *//*   
        public void SendNotifications(string message)
        {
            string connectionId = Context.ConnectionId;
            Clients.All.receiveNotification(message);
        }*/

        //Logged Use Call
        public void GetNotification()
        {
            try
            {
                string loggedUser = Context.User.Identity.Name;

                //Get TotalNotification
                string totalNotif = LoadCountNotify(loggedUser);
                var dataNotif = LoadNotifyData(loggedUser);
                //Send To
                UserHubModels receiver;
                if (Users.TryGetValue(loggedUser, out receiver))
                {
                    var cid = receiver.ConnectionIds;
                    foreach (var item in cid)
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                        context.Clients.Client(item).broadcaastNotif(totalNotif);
                        context.Clients.Client(item).notify(dataNotif);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Logged Use Call
        public void GetNotification(string loggedUser)
        {
            try
            {           
                //Get TotalNotification
                string totalNotif = LoadCountNotify(loggedUser);
                var dataNotif = LoadNotifyData(loggedUser);
                //Send To
                UserHubModels receiver;
                if (Users.TryGetValue(loggedUser, out receiver))
                {
                    var cid = receiver.ConnectionIds;
                    foreach (var item in cid)
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                        context.Clients.Client(item).broadcaastNotif(totalNotif);
                        context.Clients.Client(item).notify(dataNotif);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //Specific User Call
        public void SendNotification(string SentTo)
        {
            try
            {
                //Get TotalNotification
                string totalNotif = LoadCountNotify(SentTo);
                var dataNotif = LoadNotifyData(SentTo);
                //Send To
                UserHubModels receiver;
                if (Users.TryGetValue(SentTo, out receiver))
                {
                    var cid = receiver.ConnectionIds;
                    foreach(var item in cid)
                    {
                        var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                        context.Clients.Client(item).broadcaastNotif(totalNotif);
                        context.Clients.Client(item).notify(dataNotif);
                    }                   
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private string LoadCountNotify(string userId)
        {
            
            int total = 0;
            var query = (from n in db.Notification
                         join a in db.Annoucement on n.id_notification equals a.id
                         where n.id_notification == a.id && n.send_to== userId && n.is_read != true
                         select n)
                        .ToList();
            total = query.Count;
            return total.ToString();
        }

        private object LoadNotifyData(string userId)
        {
            var query = (from n in db.Notification
                         join a in db.Annoucement on n.id_notification equals a.id
                         where n.id_notification == a.id && n.send_to == userId
                         select new UserNotification
                         {
                             userID=  n.send_to,
                             message = a.message,
                             title= a.title,
                             date = n.create_time,
                             isRead = n.is_read
                         }).OrderByDescending(x=>x.date)
                       .ToList();
            return query;
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new UserHubModels
            {
                UserName = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.userConnected(userName);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            UserHubModels user;
            Users.TryGetValue(userName, out user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
                    if (!user.ConnectionIds.Any())
                    {
                        UserHubModels removedUser;
                        Users.TryRemove(userName, out removedUser);
                        Clients.Others.userDisconnected(userName);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
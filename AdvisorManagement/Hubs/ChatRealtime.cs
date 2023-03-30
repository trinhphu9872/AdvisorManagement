using AdvisorManagement.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Hubs
{

    [HubName("chat")]
    public class ChatRealtime : Hub
    {
        private CP25Team09Entities db = new CP25Team09Entities();


        public void Connect(string name)
        {
            Clients.Caller.name(name);
        }
        public void Hello()
        {
            Clients.All.hello();
        }


            
        public void Message(string mess)
        {
            var sql = db.AccountUser.Where(x => x.email == "phu.197pm09495@vanlanguni.vn").Select(x => x.email).ToList();
            Clients.All.message(sql[0]);
        }
    }
}
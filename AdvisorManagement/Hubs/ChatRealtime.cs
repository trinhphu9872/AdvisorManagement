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
            Clients.All.message(mess);
        }
    }
}
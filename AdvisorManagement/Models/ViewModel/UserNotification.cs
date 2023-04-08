using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class UserNotification
    {
        public string userID { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<bool> isRead { get; set; }

    }
}
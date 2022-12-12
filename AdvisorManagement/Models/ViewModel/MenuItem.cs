using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string menuName { get; set; }
        public string actionLink { get; set; }
        public int orderID { get; set; }

    }
}
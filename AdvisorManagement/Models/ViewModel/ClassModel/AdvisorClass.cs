using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class AdvisorClass
    {
        public int ID { get; set; }
        public string idClass { get; set; }
        public string idAdvisor { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int course { get; set; }
        public int amount { get; set; }
        public string semester { get; set; }

        public Nullable<System.DateTime> create_time { get; set; }
    }
}
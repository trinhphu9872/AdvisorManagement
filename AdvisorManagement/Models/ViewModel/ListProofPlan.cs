using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class ListProofPlan
    {
        public int id { get; set; }
        public string content { get; set; }
        public string proof { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string semester { get; set; }
        public string status { get; set; }
        public string creator { get; set; }
        public Nullable<int> id_browser { get; set; }

    }
}
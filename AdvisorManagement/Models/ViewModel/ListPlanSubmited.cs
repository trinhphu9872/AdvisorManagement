using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class ListPlanSubmited
    {
        public int id { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string class_code { get; set; }
        public int id_class { get; set; }
        public string semester { get; set; }
        public string status { get; set; }
    }
}
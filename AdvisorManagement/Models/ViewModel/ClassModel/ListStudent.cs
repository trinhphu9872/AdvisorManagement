using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class ListStudent
    {
        public int id { get; set; }

        public int idAcc { get; set; }
        public string idStudent { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int course { get; set; }
        public string status { get; set; }
    }
}
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class AdminCheckEvol
    {
        public int stt { get; set; }
        public string name_advisor { get; set; } 
        public int id { get; set; }

        public string class_id { get; set; }
        public string evol_sys { get; set; }
        public string eval_advisor { get; set;}
        public string eval_admin { get; set; }
        public string note { get; set; } = string.Empty;
    }
}
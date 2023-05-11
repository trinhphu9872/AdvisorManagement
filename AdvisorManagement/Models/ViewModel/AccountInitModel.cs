using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class AccountInitModel
    {
        public int role_id { get; set; }
        public string user_code { get; set; }
        public string user_name { get; set; }
        public string class_code { get; set; }
        public string class_name { get; set; }
        public string img_profile { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;

    }
}
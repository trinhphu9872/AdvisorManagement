using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel.UserModel
{
  
        public partial class AccountUserModel
        {
            public int ID { get; set; }
            public string user_code { get; set; }
            public int id_Role { get; set; }
            public string username { get; set; }
            public string gender { get; set; }
            public string phone { get; set; }
            public string address { get; set; }
            public string email { get; set; }
            public string dateofbirth { get; set; }
            public System.DateTime createtime { get; set; }
            public string picture { get; set; }
            [NotMapped]
            public HttpPostedFileBase ImageUpload { get; set; }
            public virtual Role Role { get; set; }
        }
    
}
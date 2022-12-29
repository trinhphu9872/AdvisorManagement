using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class AccountUserModel
    {
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}
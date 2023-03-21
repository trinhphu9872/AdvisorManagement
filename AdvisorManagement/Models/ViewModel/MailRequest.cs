using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class MailRequest
    {
        public string To { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
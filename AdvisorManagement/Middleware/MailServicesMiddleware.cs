using AdvisorManagement.Models.ViewModel;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Org.BouncyCastle.Math.EC.ECCurve;
using MailKit.Net.Smtp;
using System.Configuration;
using System.Web.Configuration;

namespace AdvisorManagement.Middleware
{
    public interface IMailServicesMiddleware
    {
        string MailSend(MailRequest request);
    }

    public class MailServicesMiddleware : IMailServicesMiddleware
    {
        private readonly IMailServicesMiddleware _mailCore;
        public MailServicesMiddleware(IMailServicesMiddleware mailService)
        {
            this._mailCore = mailService;
        }
        public string MailSend(MailRequest request) {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(WebConfigurationManager.AppSettings["EmailSystem"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Message };
            var smtp = new SmtpClient();
            smtp.Connect(WebConfigurationManager.AppSettings["EmailSystem"], int.Parse(WebConfigurationManager.AppSettings["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(WebConfigurationManager.AppSettings["EUserName"], WebConfigurationManager.AppSettings["EPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
            return "Ok";
        }
    }
}
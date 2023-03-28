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

    public class MailServicesMiddleware 
    {
        public string MailSend(MailRequest request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(WebConfigurationManager.AppSettings["EmailSystem"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Message };
            var smtp = new SmtpClient();
            smtp.Connect(WebConfigurationManager.AppSettings["EmailHost"], int.Parse(WebConfigurationManager.AppSettings["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(WebConfigurationManager.AppSettings["EUserName"], WebConfigurationManager.AppSettings["EPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
            return "Ok";
        }

        public string MailSendMuiltiRequest(MailRequest request, List<string> MultiMail)
        {
            var email = new MimeMessage();
            InternetAddressList listMail = new InternetAddressList();
            MultiMail.ForEach(mail =>
            {
                listMail.Add(new MailboxAddress("user", mail));
            });
            email.From.Add(MailboxAddress.Parse(WebConfigurationManager.AppSettings["EmailSystem"]));
            email.To.AddRange(listMail);
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Message };
            var smtp = new SmtpClient();
            smtp.Connect(WebConfigurationManager.AppSettings["EmailHost"], int.Parse(WebConfigurationManager.AppSettings["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(WebConfigurationManager.AppSettings["EUserName"], WebConfigurationManager.AppSettings["EPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
            return "Ok";
        }
    }
}
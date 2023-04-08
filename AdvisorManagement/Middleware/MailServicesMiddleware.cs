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
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting.Server;

namespace AdvisorManagement.Middleware
{

    public class MailServicesMiddleware 
    {
        private AccountMiddleware serviceAccount = new AccountMiddleware();

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
        // mail all

        public string MailSendMuiltiRequest(MailRequest request, List<string> MultiMail, string content)
        {
            // Load the HTML template
            string htmlTemplate = content;
            var emails = new List<MimeMessage>();
            MultiMail.ForEach(m =>
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(WebConfigurationManager.AppSettings["EmailSystem"]));
                email.To.Add(MailboxAddress.Parse(m.Trim()));
                //email.To.Add(MailboxAddress.Parse("phu.197pm09495@vanlanguni.vn"));

                email.Subject = request.Subject;
                // get name
                string nameUser = serviceAccount.getTextName(m.Trim());
                // Create the message body
                string finalHtml = htmlTemplate.Replace("[[Name]]", nameUser).Replace("[[Subject]]", request.Subject).Replace("[[message]]", request.Message);
                var builder = new BodyBuilder();
                builder.HtmlBody = finalHtml;
                email.Body = builder.ToMessageBody();
                emails.Add(email);
            });
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(WebConfigurationManager.AppSettings["EmailHost"], int.Parse(WebConfigurationManager.AppSettings["EmailPort"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(WebConfigurationManager.AppSettings["EUserName"], WebConfigurationManager.AppSettings["EPassword"]);
                foreach (var email in emails)
                {
                    smtp.Send(email);
                }
                smtp.Disconnect(true);
            }
            return "Nhắc nhở thành công";
        }

  
    }
}
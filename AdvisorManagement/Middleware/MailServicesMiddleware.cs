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

        public string MailSendMuiltiRequest(MailRequest request, List<string> MultiMail)
        {
            // Load the HTML template
            string htmlTemplate = LoadHtmlTemplate();
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
                string finalHtml = htmlTemplate.Replace("[[Name]]", nameUser).Replace("[[Subject]]", request.Subject).Replace("[[Message]]", request.Message);
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

        public string LoadHtmlTemplate()
        {
            // Load the HTML template from a file, database or other source
            // Here's an example of loading from a file:

   

            return "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body bgcolor=\"#f5f5f5\" leftmargin=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" offset=\"0\" style=\"padding:70px 0 70px 0;\">\r\n    <table width=\"600\" height=\"auto\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#fdfdfd; border:1px solid #dcdcdc; border-radius:3px !important;\">\r\n        <tr>\r\n            <td width=\"600\" height=\"auto\" bgcolor=\"#000\" border=\"0\" style=\"padding:36px 48px; display:block; margin: 0px auto;\">\r\n                <h1 style=\"color:#ffffff; font-family:&quot; Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:30px; line-height:150%; font-weight:300; margin:0; text-align:left;\">[[Subject]]</h1>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" bgcolor=\"#fdfdfd\" border=\"0\" style=\"color:#737373; font-family:&quot;Helvetica Neue&quot;,Helvetica,Roboto,Arial,sans-serif; font-size:14px; line-height:150%; text-align:left; padding:48px;\">\r\n                <p style=\"margin:0 0 16px;\">Xin chào giảng viên <b>[[Name]]</b></p>\r\n                <p style=\"margin:0 0 16px;\">[[Message]]</p>\r\n                <p style=\"margin:0 0 16px;\">Trân trọng,</p>\r\n\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td width=\"600\" border=\"0\" style=\"padding:0 48px 48px 48px; color:#707070; font-family:Arial; font-size:12px; line-height:125%; text-align:center;\">\r\n                <b>KHOA CÔNG NGHỆ THÔNG TIN - TRƯỜNG ĐẠI HỌC VĂN LANG</p>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
        }
    }
}
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using Portal.Web.Controllers;
using SendGrid;
using System.Net.Mime;

namespace Portal.Web.Models
{
    public class EmailModel
    {
        public static readonly EmailModel inst = new EmailModel();
        protected const string root = "~/App_Data/emails/";
        protected const string emailsExp = ".html";
        protected const string textExp = ".txt";
        protected const string from = "Øystein Barhaughøgda <admin@easylearn.no>";
        protected const string login = "oystein@granat.no";
        protected const string password = "Granat2012";
        protected EmailModel()
        {
        }



        public void Send(string emailTarget, string subject, string template, Dictionary<string, string> param)
        {
            try
            {

                var message = new SendGridMessage();
                message.From = new MailAddress(from);
                var recipients = new List<string>();
                recipients.Add(emailTarget);

                //{
                //    @"Tord Ertresvaag <t@j64.com>",
                //    @"Øystein Barhaughøgda <oystein@granat.no>",
                //    @"Karl-Erik Heimdal <karl@granat.no>"
                //};

                /*message.AddTo(recipients);
                message.Subject = subject;
                message.Html = ProcessTemplate(template, param);
                message.Text = ProcessTextTemplate(template, param);
                var credentials = new NetworkCredential(login, password);
                var transportWeb = new SendGrid.Web(credentials);
                message.EnableSpamCheck();
                transportWeb.Deliver(message);*/


                MailMessage mail = new MailMessage(from, emailTarget);
                SmtpClient client = new SmtpClient();
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(ProcessTemplate(template, param), null, MediaTypeNames.Text.Html);
                AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(ProcessTextTemplate(template, param), null, MediaTypeNames.Text.Plain);

                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.mandrillapp.com";
                client.Credentials = new System.Net.NetworkCredential("oystein@granat.no", "fCa8Y2f9RGTr63VGb9yLlg");

                mail.Subject = subject;
                //mail.Body = ProcessTemplate(template, param);
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(htmlView);
                mail.AlternateViews.Add(plainTextView);
                client.Send(mail);


            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }
        }

        protected string ProcessTemplate(string template, Dictionary<string, string> param)
        {
           return param.Aggregate(File.ReadAllText(HttpContext.Current.Server.MapPath(root + template + emailsExp)),
                                (current, item) => current.Replace("[" + item.Key + "]", item.Value));
        }
        protected string ProcessTextTemplate(string template, Dictionary<string, string> param)
        {
            return param.Aggregate(File.ReadAllText(HttpContext.Current.Server.MapPath(root + template + textExp)),
                                 (current, item) => current.Replace("[" + item.Key + "]", item.Value));
        }
    }
}
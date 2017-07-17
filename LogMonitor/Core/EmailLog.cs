using System;
using System.Net.Mail;
using System.Windows.Forms;

namespace LogMonitor.Core
{
   public static class EmailLog
   {
      public static void SendEmail(string body, string subject)
      {
         var to = "vildar82@gmail.com";
         var from = "KhisyametdinovVT@pik.ru";

         try
         {
            var mailServer = "ex20pik.picompany.ru";

            var mailMessage = new MailMessage(
                                                   from,
                                                   to,
                                                   subject,
                                                   body
                                                  );
            //mailMessage.IsBodyHtml = true;
            var client = new SmtpClient(mailServer);
            //client.UseDefaultCredentials = true;
            client.Send(mailMessage);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.ToString());
         }
      }
   }
}
using System;
using System.Net.Mail;
using System.Windows.Forms;

namespace LogMonitor.Core
{
   public static class EmailLog
   {
      public static void SendEmail(string body, string subject)
      {
         string to = "vildar82@gmail.com";
         string from = "KhisyametdinovVT@pik.ru";

         try
         {
            string mailServer = "ex20pik.picompany.ru";

            MailMessage mailMessage = new MailMessage(
                                                   from,
                                                   to,
                                                   subject,
                                                   body
                                                  );
            //mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(mailServer);
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
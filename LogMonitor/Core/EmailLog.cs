using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogMonitor.Core
{
   public static class EmailLog
   {
      public static void SendAutomatedEmail(string body)
      {
         string to = "vildar82@gmail.com";
         string from = "KhisyametdinovVT@pik.ru";

         try
         {
            string mailServer = "ex20pik.picompany.ru";

            MailMessage mailMessage = new MailMessage(
                                                   from,
                                                   to,
                                                   "Plugin Log Monitor",
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

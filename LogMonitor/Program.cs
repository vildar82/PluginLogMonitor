using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogMonitor.Core;

namespace LogMonitor
{   
   static class Program
   {
      [STAThread]
      static void Main()
      {
         // Периодически считывать логи из 
         // \\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs
         // Собирать логи от плагинов (Plugin).
         // Периодически отправлять сводку на почту.      
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(new FormLog());         
      }
   }
}

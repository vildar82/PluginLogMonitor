using System;
using System.Windows.Forms;

namespace LogMonitor
{
   internal static class Program
   {
      [STAThread]
      private static void Main()
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
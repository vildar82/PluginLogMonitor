using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogMonitor
{
    public static class Program
    {
        public const string FileExcelUserList = @"\\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\Users\UserList2.xlsx";

        [STAThread]
        private static void Main ()
        {
            if (IsAnyProcess())
            {
                return;
            }
            // Периодически считывать логи из
            // \\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs
            // Собирать логи от плагинов (Plugin).
            // Периодически отправлять сводку на почту.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLog());
        }

        private static bool IsAnyProcess()
        {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Skip(1).Any();
        }
    }
}
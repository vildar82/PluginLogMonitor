using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using LogMonitor.Core.AllUsers;

namespace LogMonitor
{
    public static class Program
    {
        public const string FileExcelUserList = @"\\picompany.ru\pikp\lib\_CadSettings\AutoCAD_server\Users\UserList2.xlsx";

        [STAThread]
        private static void Main ()
        {
            if (IsAnyProcess())
            {
                return;
            }
            // Регистрация пользователей ГТО
            //RegUsers("ГТО, КР-МН", "006789_Геотехнический отдел", "main.picompany.ru");

            // Периодически считывать логи из
            // \\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs
            // Собирать логи от плагинов (Plugin).
            // Периодически отправлять сводку на почту.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLog());
        }

        private static void RegUsers(string wg, string groupAD, string domain = null)
        {
            //"006789_Геотехнический отдел"
            var users = ADUtils.GetUsersInGroup(groupAD, domain);
            var fi = new FileInfo(FileExcelUserList);
            using (var excel = new OfficeOpenXml.ExcelPackage(fi))
            {
                var worksheet = excel.Workbook.Worksheets[1];
                var numberRow = 2;
                while (true)
                {
                    var login = worksheet.Cells[numberRow, 2].Text.Trim();
                    if (string.IsNullOrEmpty(login)) break;
                    var cellGroup = worksheet.Cells[numberRow, 3];
                    var group = cellGroup.Text;
                    var user = users.FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
                    if (user != null)
                    {
                        if (group != wg)
                        {
                            cellGroup.Value = wg;
                        }
                        users.Remove(user);
                    }
                    numberRow++;
                }
                foreach (var userInfo in users)
                {
                    worksheet.Cells[numberRow, 1].Value = userInfo.Name;
                    worksheet.Cells[numberRow, 2].Value = userInfo.Login;
                    worksheet.Cells[numberRow, 3].Value = wg;
                    numberRow++;
                }
                excel.Save();
            }
        }

        private static bool IsAnyProcess()
        {
            return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Skip(1).Any();
        }
    }
}
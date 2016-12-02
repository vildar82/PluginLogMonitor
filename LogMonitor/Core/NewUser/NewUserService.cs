using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMonitor.Core.AllUsers;

namespace LogMonitor.Core.NewUser
{
    public static class NewUserService
    {
        private static List<string> _checkedUsers = new List<string> ();
        private static List<string> _ecpWorkGroups =ADUtils.GetEcpWorkGroups();
        public static List<NewUserInfo> NewUsers { get; } = new List<NewUserInfo>();
        public static List<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Проверка нового пользователя
        /// </summary>
        /// <param name="msgLine">Строка лога</param>
        public static void CheckNewUser (string msgLine, string userName)
        {
            try
            {
                if (isNewUserLogLine(msgLine))
                {
                    // Проверка проверялся ли уже этот пользователь в этом сеансе                    
                    if (isCheckedUser(userName)) return;

                    NewUserInfo user = new NewUserInfo (userName);
                    
                    // Проверка есть ли запись этого юзера в списке UserList2.xlsx
                    if (isUserExistInExcelUserList(user)) return;                    

                    // Определение рабочей группы пользователя
                    if (DefineUserGroup(user))
                    {
                        // Регистрация пользователя - добавление в файл UserList2.xlsx
                        RegisterNewUser(user);
                        NewUsers.Add(user);
                    }
                    else
                    {
                        // Не определена рабочая группа пользовтеля - не добавлен в группу ЕЦП
                        Errors.Add($"Пользователь без группы ЕЦП: user={userName};");
                    }       
                         
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"Ошибка обработки лога пользователя {userName}. Лог {msgLine}. Ошибка - {ex}");
            }
        }        

        /// <summary>
        /// Проверка - это строка лога про нового пользователя - которого нет в группе в списке UserList2.xlsx
        /// </summary>
        /// <param name="msgLine">Строка лога</param>
        /// <returns>Да/нет - это строка лога про нового пользователя</returns>
        private static bool isNewUserLogLine (string msgLine)
        {
            if (msgLine.StartsWith("Не определена группа") ||
                msgLine.StartsWith("Не определена рабочая группа"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Проверялся ли этот пользователь уже.        
        /// </summary>        
        private static bool isCheckedUser (string userName)
        {
            var res = _checkedUsers.Contains(userName, StringComparer.OrdinalIgnoreCase);
            if (!res) _checkedUsers.Add(userName);
            return res;
        }

        private static bool isUserExistInExcelUserList (NewUserInfo user)
        {
            string fileUsers = Program.FileExcelUserList;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(fileUsers))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();

                var worksheet = pck.Workbook.Worksheets[1];

                int numberRow = 2;
                while (worksheet.Cells[numberRow, 2].Text.Trim() != "")
                {
                    if (worksheet.Cells[numberRow, 2].Text.Trim().Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    numberRow++;
                }
            }
            return false;
        }

        private static void RegisterNewUser (NewUserInfo user)
        {
            string fileUsers = Program.FileExcelUserList;
            using (var pck = new OfficeOpenXml.ExcelPackage(new FileInfo(fileUsers)))
            {                
                var ws = pck.Workbook.Worksheets.First();

                var worksheet = pck.Workbook.Worksheets[1];

                int numberRow = 2;
                while (worksheet.Cells[numberRow, 2].Text.Trim() != "")
                {                    
                    numberRow++;
                }
                worksheet.Cells[numberRow, 1].Value = user.FIO;
                worksheet.Cells[numberRow, 2].Value = user.UserName;
                worksheet.Cells[numberRow, 3].Value = user.WorkGroup;

                pck.Save();
            }
        }

        /// <summary>
        /// Определение рабочей группы пользователя
        /// </summary>
        /// <returns></returns>
        private static bool DefineUserGroup (NewUserInfo user)
        {   
            string fio;
            var userGroups = ADUtils.GetUserGroups(user.UserName, out fio);
            user.FIO = fio;
            var userWorkGroups = _ecpWorkGroups.Intersect(userGroups);

            user.WorkGroups = userWorkGroups;

            foreach (var item in userWorkGroups)
            {
                // определить шифр отдела по рабочей группе
                var wg = getWorkGroupName(item);
                if (!string.IsNullOrEmpty(wg))
                {
                    user.WorkGroup = wg;
                    return true;
                }
            }                                                    
            return false;
        }

        private static string getWorkGroupName (string group)
        {
            if (group.Contains("_GP") || group.Contains("_KG")) return "ГП";
            if (group.Contains("_AR") || group.Contains("_TM")) return "АР";
            if (group.Contains("_AK")) return "АК";
            if (group.Contains("_EO")) return "ЭО";
            if (group.Contains("_OV")) return "ОВ";
            if (group.Contains("_SS")) return "СС";
            if (group.Contains("_VK")) return "ВК";
            if (group.Contains("_KR_MN")) return "КР-МН";
            if (group.Contains("_KR_NR")) return "КР-СБ-ГК";
            if (group.Contains("_KR_SB")) return "КР-СБ";
            if (group.Contains("_RP")) return "РП";            
            return null;
        }
    }
}

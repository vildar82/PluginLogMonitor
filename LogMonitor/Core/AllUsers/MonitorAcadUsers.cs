using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogMonitor.Core.AllUsers
{
    /// <summary>
    /// Парсер логов AutoCAD_PIK_Manager
    /// </summary>
    public class MonitorAcadUsers
    {
        public string Report { get; private set; } = string.Empty;
        public string LogFolder { get; set; } = @"\\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs";
        public int DaysLookingFor { get; set; } = 31;
        public string GroupAD { get; set; } = "adm-dsk3-AutoCADSettings-u";

        // Все пользователи в группе AD
        public List<UserInfo> UsersAD { get; private set; } = new List<UserInfo>();

        // Пользователи с ошибками в логе
        public List<UserInfo> UsersErorInLog { get; private set; } = new List<UserInfo>();

        // Пользователи которых нет в группе АД, но есть в логах
        public List<UserInfo> UsersErorInLogNotInAD { get; private set; } = new List<UserInfo>();

        // Пользователи которых нет в логах
        public List<UserInfo> UsersErorNotInLog { get; private set; } = new List<UserInfo>();

        // Пользователи у которых давно нет успешной установки настроек
        public List<UserInfo> UsersErorLongSuccess { get; private set; } = new List<UserInfo>();

        // Пользователи в логах у которых все ОК
        public List<UserInfo> UsersLog { get; private set; } = new List<UserInfo>();

        public void CheckAllUsers()
        {
            Parse();
            SendReport();
        }

        private void Parse()
        {
            UsersAD = ADUtils.GetUsersInGroup(GroupAD);
            DirectoryInfo dirLogInfo = new DirectoryInfo(LogFolder);
            var logFiles = dirLogInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly).OrderByDescending(f=>f.LastWriteTime);
            // Поиск пользователей в логах
            foreach (var log in logFiles)
            {   
                getUserInLog(log.FullName);
            }
            // Проверка пользователей
            CheckUsersInLog();
        }

        private void SendReport()
        {
            //Отправка отчета
            Report = ReportAllUsers.GetReportMessage(this);
            EmailLog.SendEmail(Report, "Log Monitor All Users AutoCAD Settings");
        }

        private static void findLastError(UserInfo userlog, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var logType = line.Split('_');
                if (logType.Length > 1)
                {
                    if (logType[1].StartsWith("Error"))
                    {
                        userlog.LastError += line;
                        break;
                    }
                }
            }
        }

        private static void findUserGroupAcad(UserInfo userlog, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                string valueSearch = string.Format("{0} Группа - ", userlog.Login);
                var index = line.IndexOf(valueSearch, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    userlog.GroupAcad = line.Substring(index + valueSearch.Length).Trim();
                }
            }
        }

        private void findLastSuccessSetting(UserInfo userlog, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.Contains("Профиль ПИК установлен"))
                {
                    var logType = line.Split('_');
                    if (logType.Length > 1)
                    {
                        var lastSuccesSetting = DateTime.Parse(logType[0]);
                        if (lastSuccesSetting > userlog.LastSuccesSetting)
                        {
                            userlog.LastSuccesSetting = lastSuccesSetting;
                        }
                        break;
                    }
                }
            }
        }

        private List<UserInfo> checkUserNotLog()
        {
            List<UserInfo> checkUserNotLog = new List<UserInfo>();
            foreach (var userAd in UsersAD)
            {
                if (!UsersLog.Exists(u => isEqualLogins(u.Login, userAd.Login)))
                {
                    checkUserNotLog.Add(userAd);
                }
            }
            return checkUserNotLog;
        }

        private void CheckUsersInLog()
        {
            UsersErorNotInLog = checkUserNotLog();

            foreach (var userLog in UsersLog)
            {
                if (userLog.LastSuccesSetting == DateTime.MinValue)
                {
                    // Нет успешного завершения настроек
                    UsersErorInLog.Add(userLog);
                }
                else
                {
                    // Давно нет успешной установки настроек
                    var daysForLastSucces = (DateTime.Now - userLog.LastSuccesSetting).Days;
                    if (daysForLastSucces > DaysLookingFor)
                    {
                        UsersErorLongSuccess.Add(userLog);
                    }
                }
            }
            //UsersErorInLog.ForEach(e => UsersLog.Remove(e));
            //UsersErorLongSuccess.ForEach(e => UsersLog.Remove(e));
        }

        private string getLoginByFileLogNam(string log)
        {
            string res = Path.GetFileNameWithoutExtension(log);
            int indexC = res.IndexOf("-C-"); // англ C
            if (indexC == -1)
            {
                indexC = res.IndexOf("-С-"); // рус С
                if (indexC == -1)
                {
                    return res;
                }
            }
            return res.Substring(0, indexC);
        }

        private void getUserInLog(string log)
        {
            // Определение пользователя по имени лог файла
            string loginByLog = getLoginByFileLogNam(log);
            UserInfo userlog = UsersLog.Find(u => isEqualLogins(loginByLog, u.Login));
            if (userlog == null)
            {
                userlog = new UserInfo(loginByLog, loginByLog, "");
                UsersLog.Add(userlog);
                // Поиск пользователя в списке пользователей AD
                var userAD = UsersAD.Find(u => isEqualLogins(loginByLog, u.Login));
                if (userAD == null)
                {
                    // Пользователь которого нет в списке группы АД - возможно это старый пользователь, которого уволили или типа того.
                    UsersErorInLogNotInAD.Add(userlog);
                }
                else
                {
                    userlog.Name = userAD.Name;
                    userlog.Login = userAD.Login;
                    userlog.GroupAD = userAD.GroupAD;
                }

                var lines = File.ReadAllLines(log, Encoding.Default).Reverse();
                // поиск строки успешного выполнения настроек - "Профиль ПИК установлен."
                findLastSuccessSetting(userlog, lines);
                // Поиск последней ошибки
                findLastError(userlog, lines);
                // Поиск группы AutoCAD (шифр отдела)
                findUserGroupAcad(userlog, lines);
                // Версия Net Framework
                findNetVersion(userlog, lines);
                //Версия автокада
                findAcadVer(userlog, lines);
            }            
        }

        private void findAcadVer (UserInfo userlog, IEnumerable<string> lines)
        {
            foreach (var line in lines.Reverse())
            {
                string searchInput = "Версия автокада -";
                var index = line.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    var value = line.Substring(index + searchInput.Length).Trim();
                    try
                    {
                        Version ver = Version.Parse(value);
                        if (userlog.AcadVersion == null || userlog.AcadVersion < ver)
                        {
                            userlog.AcadVersion = ver;
                        }
                    }
                    catch { }
                    return;
                }
            }
        }

        private void findNetVersion(UserInfo userlog, IEnumerable<string> lines)
        {
            foreach (var line in lines.Reverse())
            {
                string searchInput = "Версия среды .NET Framework -";
                var index = line.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {                    
                    var value = line.Substring(index + searchInput.Length).Trim();
                    try
                    {
                        Version ver = Version.Parse(value);
                        if (userlog.NetVersion == null || userlog.NetVersion < ver)
                        {
                            userlog.NetVersion = ver;
                        }
                    }
                    catch { }
                    return;
                }
            }
        }

        private bool isEqualLogins(string loginByLog, string login)
        {
            return loginByLog.IndexOf(login, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogMonitor.Core.AddNewUsers;
using LogMonitor.Core.NewUser;

namespace LogMonitor.Core.AllUsers
{
    /// <summary>
    /// Парсер логов AutoCAD_PIK_Manager
    /// </summary>
    public class MonitorAcadUsers
    {
        private object lockUsersLog = new object();
        public string Report { get; private set; } = string.Empty;
        public string LogFolder { get; set; } = LogService._logPath;
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
            var dirLogInfo = new DirectoryInfo(LogFolder);
            var logFiles = dirLogInfo.GetFiles("*.log", SearchOption.TopDirectoryOnly).OrderByDescending(f=>f.LastWriteTime);
            // Поиск пользователей в логах

            Parallel.ForEach(logFiles, l => GetUserInLog(l.FullName));
            //foreach (var log in logFiles)
            //{   
            //    GetUserInLog(log.FullName);
            //}
            // Проверка пользователей
            CheckUsersInLog();

            UsersAD.Sort();
            UsersErorInLog.Sort();
            UsersErorInLogNotInAD.Sort();
            UsersErorNotInLog.Sort();
            UsersErorLongSuccess.Sort();
            UsersLog.Sort();
        }

        private void SendReport()
        {
            //Отправка отчета
            Report = ReportAllUsers.GetReportMessage(this);
            EmailLog.SendEmail(Report, "Log Monitor All Users AutoCAD Settings");
        }

        private static void FindLastError(UserInfo userlog, [NotNull] IEnumerable<string> lines)
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

        private static void FindUserGroupAcad(UserInfo userlog, [NotNull] IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var valueSearch = $"{userlog.Login} Группа - ";
                var index = line.IndexOf(valueSearch, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    userlog.GroupAcad = line.Substring(index + valueSearch.Length).Trim();
                }
            }
        }

        private void FindLastSuccessSetting(UserInfo userlog, [NotNull] IEnumerable<string> lines)
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

        [NotNull]
        private List<UserInfo> CheckUserNotLog()
        {
            var checkUserNotLog = new List<UserInfo>();
            foreach (var userAd in UsersAD)
            {
                if (!UsersLog.Exists(u => IsEqualLogins(u.Login, userAd.Login)))
                {
                    checkUserNotLog.Add(userAd);
                }
            }
            return checkUserNotLog;
        }

        private void CheckUsersInLog()
        {
            UsersErorNotInLog = CheckUserNotLog();

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

        [NotNull]
        private static string GetLoginByFileLogNam(string log)
        {
            var res = Path.GetFileNameWithoutExtension(log);
            var indexC = res.IndexOf("-C-"); // англ C
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

        private void GetUserInLog(string log)
        {
            // Определение пользователя по имени лог файла
            var loginByLog = GetLoginByFileLogNam(log);
            UserInfo userlog;
            lock (lockUsersLog)
            {
                userlog = UsersLog.Find(u => IsEqualLogins(loginByLog, u.Login));
            }
            if (userlog == null)
            {
                userlog = new UserInfo(loginByLog, loginByLog, "");
                UsersLog.Add(userlog);
                // Поиск пользователя в списке пользователей AD
                var userAD = UsersAD.Find(u => IsEqualLogins(loginByLog, u.Login));
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
                FindLastSuccessSetting(userlog, lines);
                // Поиск последней ошибки
                FindLastError(userlog, lines);
                // Поиск группы AutoCAD (шифр отдела)
                FindUserGroupAcad(userlog, lines);
                // Версия Net Framework
                FindNetVersion(userlog, lines);
                //Версия автокада
                FindAcadVer(userlog, lines);
            }            
        }

        private static void FindAcadVer (UserInfo userlog, [NotNull] IEnumerable<string> lines)
        {
            foreach (var line in lines.Reverse())
            {
                var searchInput = "Версия автокада -";
                var index = line.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {
                    var value = line.Substring(index + searchInput.Length).Trim();
                    try
                    {
                        var ver = Version.Parse(value);
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

        private static void FindNetVersion(UserInfo userlog, [NotNull] IEnumerable<string> lines)
        {
            foreach (var line in lines.Reverse())
            {
                var searchInput = "Версия среды .NET Framework -";
                var index = line.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase);
                if (index > 0)
                {                    
                    var value = line.Substring(index + searchInput.Length).Trim();
                    try
                    {
                        var ver = Version.Parse(value);
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

        private static bool IsEqualLogins([NotNull] string loginByLog, [NotNull] string login)
        {
            return loginByLog.IndexOf(login, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
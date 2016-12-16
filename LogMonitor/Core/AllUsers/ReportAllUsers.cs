using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonitor.Core.AllUsers
{
    public static class ReportAllUsers
    {
        public static string GetReportMessage(MonitorAcadUsers apm)
        {
            StringBuilder report = new StringBuilder("Отчет по всем пользователям настроек AutoCAD\n");

            report.AppendLine($"\n:Версии .NET Framework:");
            var netVers = apm.UsersLog.GroupBy(u => u.NetVersion).OrderByDescending(g => g.Count());            
            foreach (var ver in netVers)
            {
                report.AppendLine($"\t{ver.Key} - {ver.Count()}");
            }

            report.AppendLine($"\n:Версии AutoCAD:");
            var acadVers = apm.UsersLog.GroupBy(u => u.AcadVersion).OrderByDescending(g => g.Count());
            foreach (var ver in acadVers)
            {
                report.AppendLine($"\t{ver.Key} - {ver.Count()}");
            }

            report.AppendFormat("\n\nГруппа AD: {0}, Папка логов: {1}\n", apm.GroupAD, apm.LogFolder);
            report.AppendFormat("\nUsersAD Пользователей в группе AD настроек AutoCAD: {0}", apm.UsersAD.Count);
            report.AppendFormat("\nUsersLog Пользователей в логах у которых все ок: {0}", apm.UsersLog.Count);
            report.AppendLine("\n\nПользователей с ошибками");
            report.AppendFormat("\nUsersErorNotInLog Пользователей которых нет в логах (не произошла настройка вообще): {0}",
                                 apm.UsersErorNotInLog.Count);
            report.AppendFormat("\nUsersErorInLog Пользователей с ошибками в логе: {0}", apm.UsersErorInLog.Count);
            report.AppendFormat("\nUsersErorInLogNotInADПользователей которых нет в AD но есть логи (уволенные или типа того): {0}",
                                 apm.UsersErorInLogNotInAD.Count);
            report.AppendFormat("\nUsersErorLongSuccessПользователей у которых давно не выполнялась настройка (может комп сменился или типа того): {0}",
                                 apm.UsersErorLongSuccess.Count);
            report.AppendLine("\nПодробнее:");
            report.Append(getReportForUsers(apm.UsersErorNotInLog, "UsersErorNotInLog - которых нет в логах (не произошла настройка вообще)\n"));
            report.Append(getReportForUsers(apm.UsersErorInLog, "UsersErorInLog Пользователей с ошибками в логе\n"));
            report.Append(getReportForUsers(apm.UsersErorInLogNotInAD, "UsersErorInLogNotInADПользователей которых нет в AD но есть логи (уволенные или типа того)\n"));
            report.Append(getReportForUsers(apm.UsersErorLongSuccess, "UsersErorLongSuccessПользователей у которых давно не выполнялась настройка (может комп сменился или типа того)\n"));

            return report.ToString();
        }

        private static string getReportForUsers(List<UserInfo> users, string title)
        {            
            StringBuilder res = new StringBuilder();
            res.AppendLine();
            res.AppendLine();
            res.AppendLine(title);
            res.AppendLine();            
            int i = 1;            
            foreach (var user in users)
            {
                res.AppendFormat("{0} {1}\n", i++, user.ToString());
            }

            return res.ToString();
        }
    }
}

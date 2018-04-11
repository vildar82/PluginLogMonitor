using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using LogMonitor.Core.AllUsers;

namespace LogMonitor.Core.NewUser
{
    public static class NewUserService
    {
		private static Dictionary<string, string> dictWorkgroup = new Dictionary<string, string>();
        private static readonly List<string> _checkedUsers = new List<string> ();
        private static readonly List<string> _ecpWorkGroups =ADUtils.GetEcpWorkGroups();
	    private static object lockExcelUserList = new object();
	    public static List<NewUserInfo> NewUsers { get; } = new List<NewUserInfo>();
	    public static List<UserInfo> addNewUsers { get; } = new List<UserInfo>();
		public static List<string> Errors { get; } = new List<string>();

	    public static void RegNewUser(string userLogin, string userGroup)
	    {
		    var user = new NewUserInfo(userLogin) { WorkGroup = userGroup};
			// Проверка групп пользователя
		    var userGroupsAD = ADUtils.GetUserGroups(user.UserName, out var fio);
		    if (userGroupsAD == null)
		    {
			    MessageBox.Show($@"Не найден юзер '{userLogin}'");
		    }
			user.Position = userGroupsAD?.Position;
		    user.Department = userGroupsAD?.Department;
		    user.FIO = fio;
		    var userGroupAD = GetGroupADName(userGroup);
		    if (userGroupAD == null)
		    {
			    MessageBox.Show($@"Не определена специальность пользователя по группе {userGroup} юзер {userLogin}");
		    }
		    else if (!userGroupsAD.Groups.Any(g => g.Equals(userGroupAD, StringComparison.OrdinalIgnoreCase)))
		    {
			    // добавить пользователя в группу AD
			    ADUtils.AddUserToGroup(user.UserName, userGroupAD);
		    }
			// Проверка есть ли запись этого юзера в списке UserList2.xlsx
			if (!IsUserExistInExcelUserList(user.UserName, out var group))
		    {
			    RegisterNewUserInExcelUserList(user);
			}
		}

	    /// <summary>
	    /// Проверка нового пользователя
	    /// </summary>
	    /// <param name="msgLine">Строка лога</param>
	    /// <param name="userName"></param>
	    public static void CheckNewUserInMsgLine (string msgLine, string userName)
        {
            try
            {
                if (IsNewUserLogLine(msgLine))
                {
	                CheckNewUser(userName);
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"Ошибка обработки лога пользователя {userName}. Лог {msgLine}. Ошибка - {ex}");
            }
        }

	    private static bool CheckNewUser(string userName)
	    {
			// Проверка проверялся ли уже этот пользователь в этом сеансе                    
		    if (IsCheckedUser(userName)) return true;

		    var user = new NewUserInfo(userName);

		    // Проверка есть ли запись этого юзера в списке UserList2.xlsx
		    if (IsUserExistInExcelUserList(user.UserName, out var _)) return true;

		    // Определение рабочей группы пользователя
		    var userInfo = DefineUserGroup(user);
		    user.Position = userInfo.Position;
		    user.Department = userInfo.Department;
		    if (user.WorkGroup != null)
		    {
			    // Регистрация пользователя - добавление в файл UserList2.xlsx
			    RegisterNewUserInExcelUserList(user);
			    NewUsers.Add(user);
		    }
		    else
		    {
			    // Не определена рабочая группа пользовтеля - не добавлен в группу ЕЦП
			    Errors.Add($"Пользователь без группы ЕЦП: user={userName};");
				addNewUsers.Add(userInfo);
		    }
		    return false;
	    }

	    /// <summary>
        /// Проверка - это строка лога про нового пользователя - которого нет в группе в списке UserList2.xlsx
        /// </summary>
        /// <param name="msgLine">Строка лога</param>
        /// <returns>Да/нет - это строка лога про нового пользователя</returns>
        private static bool IsNewUserLogLine ([NotNull] string msgLine)
	    {
		    return msgLine.StartsWith("Не определена группа") ||
		           msgLine.StartsWith("Не определена рабочая группа");
	    }

        /// <summary>
        /// Проверялся ли этот пользователь уже.        
        /// </summary>        
        private static bool IsCheckedUser (string userName)
        {
            var res = _checkedUsers.Contains(userName, StringComparer.OrdinalIgnoreCase);
            if (!res) _checkedUsers.Add(userName);
            return res;
        }

        public static bool IsUserExistInExcelUserList (string user, out string group)
        {
	        group = string.Empty;
	        lock (lockExcelUserList)
	        {
		        using (var pck = new OfficeOpenXml.ExcelPackage())
		        {
			        using (var stream = File.OpenRead(Program.FileExcelUserList))
			        {
				        pck.Load(stream);
			        }
			        var worksheet = pck.Workbook.Worksheets[1];
			        var numberRow = 2;
			        while (worksheet.Cells[numberRow, 2].Text.Trim() != "")
			        {
				        if (worksheet.Cells[numberRow, 2].Text.Trim().Equals(user, StringComparison.OrdinalIgnoreCase))
				        {
					        group = worksheet.Cells[numberRow, 3].Text;
					        return true;
				        }
				        numberRow++;
			        }
		        }
	        }
	        return false;
        }

        private static void RegisterNewUserInExcelUserList ([NotNull] NewUserInfo user)
        {
	        lock (lockExcelUserList)
	        {
		        using (var pck = new OfficeOpenXml.ExcelPackage(new FileInfo(Program.FileExcelUserList)))
		        {
			        var worksheet = pck.Workbook.Worksheets[1];
			        var numberRow = 2;
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
        }

        /// <summary>
        /// Определение рабочей группы пользователя
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private static UserInfo DefineUserGroup ([NotNull] NewUserInfo user)
        {
			var userInfo = ADUtils.GetUserGroups(user.UserName, out var fio);
	        if (userInfo == null)
	        {
		        MessageBox.Show($@"Не найден юзер {user.UserName}");
		        return new UserInfo(user.FIO, user.UserName, user.WorkGroup);
	        }
			user.FIO = fio;
            user.WorkGroups = userInfo.Groups;
	        if (!dictWorkgroup.TryGetValue(userInfo.Position, out var wg))
	        {
		        wg = GetWorkGroupName(userInfo.Position);
		        dictWorkgroup.Add(userInfo.Position, wg);
	        }
	        user.WorkGroup = wg;
	        return userInfo;
        }

	    [CanBeNull]
	    private static string GetWorkGroupName([NotNull] string position)
	    {
		    if (position.Contains("генерального плана")) return "ГП";
		    if (position.Contains("Архитектор") || position.Contains("архитект")) return "АР";
		    if (position.StartsWith("внешних сетей")) return "НС";
		    if (position.Contains("сигнализ") || position.Contains("слаботоч") ||
		        position.Contains("автоматиз") || position.Contains("СС")) return "СС";
		    if (position.Contains("электросн")) return "ЭО";
		    if (position.Contains("отопления") || position.Contains("ОВ") ||
		        position.Contains("тепломех") || position.Contains("тепловых")) return "ОВ";
		    if (position.Contains("водоснабж") || position.Contains("ВВ")) return "ВК";
		    if (position.Contains("конструктор")) return "КР-МН";
		    if (position.Contains("сборному")) return "КР-СБ";
		    if (position.Contains("Архивариус-делопроизводитель")) return "ДО";
		    if (position.Contains("Руководитель проекта")) return "РП";
		    return "АР";
	    }

	    [CanBeNull]
        private static string GetWorkGroupNameByGroup ([NotNull] string group)
        {
            if (group.Contains("_GP") || group.Contains("_KG")) return "ГП_Тест";
            if (group.Contains("_AR")) return "АР";
	        if (group.StartsWith("006810") || group.Contains("_NS")) return "НС";
            if (group.Contains("_AK")) return "СС";
            if (group.Contains("_EO")) return "ЭО";
            if (group.Contains("_OV")) return "ОВ";
            if (group.Contains("_SS")) return "СС";
            if (group.Contains("_VK")) return "ВК";
	        if (group.Contains("_TM")) return "ОВ";
			if (group.Contains("_KR_MN")) return "КР-МН";
            if (group.Contains("_KR_NR")) return "КР-МН";
            if (group.Contains("_KR_SB")) return "КР-СБ";
            if (group.Contains("_RP")) return "РП";            
            return null;
        }

	    [CanBeNull]
	    public static string GetGroupADName(string workGroup)
	    {
			switch (workGroup)
			{
				case "ГП": return "fld-ECP_AR_GP-u";
				case "ГП_Тест": return "fld-ECP_AR_GP-u";
				case "АР": return "fld-ECP_AR_NR-u";
				case "СС": return "fld-ECP_INZH_SS-u";
				case "ЭО": return "fld-ECP_INZH_EO-u";
				case "ОВ": return "fld-ECP_INZH_OV-u";
				case "ВК": return "fld-ECP_INZH_VK-u";
				case "КР-МН": return "fld-ECP_KR_MN-u";
				case "КР-СБ": return "fld-ECP_KR_SB-u";
				case "КР-СБ-ГК": return "fld-ECP_KR_SB-u";
				case "ЖБК-ТО": return "fld-ecp-u";
				case "ТО": return "fld-ECP_KR_SB-u";
				case "ГТО": return "fld-ECP_KR_MN-u";
				case "ДО": return "fld-ECP_DO-u";
				case "НС": return "fld-ECP_AR_GP-u";
				default: return null;
			}
		}
	}
}

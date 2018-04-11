using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace LogMonitor.Core.AllUsers
{
    public static class ADUtils
    {
	    private const string domainName = "picompany.ru";
	    private const string domainMain = "main.picompany.ru";
		private const string domainDsk2 = "dsk2.picompany.ru";

		private static Dictionary<string, UserInfo> dictUserInfo = new Dictionary<string, UserInfo>();

        /// <summary>
        /// Получить базовый основной контекст
        /// </summary>        
        [NotNull]
        public static PrincipalContext GetPrincipalContext ([CanBeNull] string domain = null)
        {
	        return domain == null
		        ? new PrincipalContext(ContextType.Domain)
		        : new PrincipalContext(ContextType.Domain, domain);
        }

		/// <summary>
		/// Получить указанного пользователя Active Directory
		/// </summary>
		/// <param name="userName">Имя пользователя для извлечения</param>        
		[CanBeNull]
		public static UserPrincipal GetUser (string userName)
		{
			var res = GetUser(userName, null);
			if (res != null) return res;
			res = GetUser(userName, domainName);
			if (res != null) return res;
			res = GetUser(userName, domainDsk2);
			return res ?? GetUser(userName, domainMain);
		}

	    [CanBeNull]
	    private static UserPrincipal GetUser([NotNull] string sUserName, string domain)
	    {
		    return UserPrincipal.FindByIdentity(GetPrincipalContext(domain), IdentityType.SamAccountName, sUserName);
	    }

	    [NotNull]
	    public static List<UserInfo> GetUsersInGroup ([NotNull] string groupName, string domain = null)
	    {
		    if (domain == null) domain = domainName;
            //string groupName = "adm-dsk3-AutoCADSettings-u";
            var users = new List<UserInfo>();
	        using (var ctx = GetPrincipalContext(domain))
	        {
		        var group = GroupPrincipal.FindByIdentity(ctx, groupName);
		        if (group != null)
		        {
			        var usersHash = new HashSet<UserInfo>();
			        IterateGroup(group, usersHash);
			        users = usersHash.ToList();
		        }
		        else
		        {
			        MessageBox.Show($"Группа не найдена - {groupName}");
		        }
		        return users;
	        }
        }

        /// <summary>
        /// Список рабочих групп ЕЦП (по специальностям) - fld-dsk3-ECP_AR_KD-u и т.п.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static List<string> GetEcpWorkGroups ()
        {
            var res = new List<string> ();
            var entry = new DirectoryEntry("LDAP://OU=ECP,OU=Groups,DC=PICompany,DC=ru");
            var ds = new DirectorySearcher( entry,"objectClass=group", null, SearchScope.OneLevel);

            using (var src = ds.FindAll())
            {                
                foreach (SearchResult sr in src)
                {
                    var name = sr.Properties["name"][0].ToString();
                    if (name.StartsWith("fld-ECP_", StringComparison.OrdinalIgnoreCase))
                    {
                        res.Add(name);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Список групп пользователя
        /// </summary>        
        [CanBeNull]
        public static UserInfo GetUserGroups ([NotNull] string userName, out string fio)
        {
			if (!dictUserInfo.TryGetValue(userName, out var userInfo))
	        {
		        using (var oUserPrincipal = GetUser(userName))
		        {
			        if (oUserPrincipal == null)
			        {
				        fio = "";
				        return null;
			        }
			        fio = oUserPrincipal.DisplayName;
			        var de = (DirectoryEntry) oUserPrincipal.GetUnderlyingObject();
			        var department = de.Properties["department"];
			        var position = de.Properties["title"];
			        userInfo = new UserInfo(fio,userName, "")
			        {
				        Department = department.Value?.ToString(),
				        Position = position.Value?.ToString(),
				        Groups = oUserPrincipal.GetGroups().Select(s => s.Name).ToList()
			        };
		        }
	        }
	        fio = userInfo.Name;
	        return userInfo;
        }

        private static void IterateGroup ([NotNull] GroupPrincipal group, HashSet<UserInfo> usersHash)
        {
            foreach (var p in group.GetMembers())
            {
                if (p is GroupPrincipal g)
                {
                    IterateGroup(g, usersHash);
                }
                else if (p is UserPrincipal n)
                {
                    var u = new UserInfo(n.DisplayName, ((UserPrincipal)p).SamAccountName, group.Name);
                    usersHash.Add(u);
                }
            }
        }

	    public static void AddUserToGroup(string userName, [NotNull] string groupAd)
	    {
		    using (var context = GetPrincipalContext(domainName))
		    {
			    var user = GetUser(userName);
			    if (user == null)
				    throw new Exception($"Пользователь не найден в AD - {userName}");
			    var group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupAd);
			    if (group == null)
				    throw new Exception($"Группа не найдена в AD - {groupAd}");
			    if (!group.Members.Contains(user))
			    {
				    group.Members.Add(user);
					group.Save();
			    }
		    }
	    }
    }
}
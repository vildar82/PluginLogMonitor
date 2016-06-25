using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace LogMonitor.Core.AllUsers
{
    public static class ADUtils
    {
        const string domainName = "picompany.ru";
        const string domainDsk2 = "dsk2.picompany.ru";

        /// <summary>
        /// Получить базовый основной контекст
        /// </summary>        
        public static PrincipalContext GetPrincipalContext (string domain = domainName)
        {
            return new PrincipalContext(ContextType.Domain, domain);
        }

        /// <summary>
        /// Получить указанного пользователя Active Directory
        /// </summary>
        /// <param name="sUserName">Имя пользователя для извлечения</param>        
        public static UserPrincipal GetUser (string sUserName)
        {
            PrincipalContext context = GetPrincipalContext();
            var res = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, sUserName);
            if (res == null)
            {
                context = GetPrincipalContext(domainDsk2);
                res = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, sUserName);
            }
            return res;
        }

        public static List<UserInfo> GetUsersInGroup (string groupName)
        {
            //string groupName = "adm-dsk3-AutoCADSettings-u";
            List<UserInfo> users = new List<UserInfo>();
            var ctx = GetPrincipalContext();            
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);
            if (group != null)
            {
                HashSet<UserInfo> usersHash = new HashSet<UserInfo>();
                IterateGroup(group, usersHash);
                users = usersHash.ToList();
            }
            else
            {
                throw new Exception("Группа не найдена");
            }
            return users;
        }

        /// <summary>
        /// Список рабочих групп ЕЦП (по специальностям) - fld-dsk3-ECP_AR_KD-u и т.п.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetEcpWorkGroups ()
        {
            List<string> res = new List<string> ();
            DirectoryEntry entry = new DirectoryEntry("LDAP://OU=ECP,OU=Groups,DC=PICompany,DC=ru");
            DirectorySearcher ds = new DirectorySearcher( entry,"objectClass=group", null, SearchScope.OneLevel);

            using (var src = ds.FindAll())
            {                
                foreach (SearchResult sr in src)
                {
                    var name = sr.Properties["name"][0].ToString();
                    if (name.StartsWith("fld-dsk3-ECP_", StringComparison.OrdinalIgnoreCase))
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
        public static List<string> GetUserGroups (string userName, out string fio)
        {
            List<string> myItems = new List<string>();
            UserPrincipal oUserPrincipal = GetUser(userName);
            fio = oUserPrincipal.DisplayName;
            using (PrincipalSearchResult<Principal> oPrincipalSearchResult = oUserPrincipal.GetGroups())
            {
                foreach (Principal oResult in oPrincipalSearchResult)
                {
                    myItems.Add(oResult.Name);
                }
            }
            return myItems;
        }

        private static void IterateGroup (GroupPrincipal group, HashSet<UserInfo> usersHash)
        {
            foreach (Principal p in group.GetMembers())
            {
                if (p is GroupPrincipal)
                {
                    IterateGroup((GroupPrincipal)p, usersHash);
                }
                else if (p is UserPrincipal)
                {
                    UserInfo u = new UserInfo(((UserPrincipal)p).DisplayName, ((UserPrincipal)p).SamAccountName, group.Name);
                    usersHash.Add(u);
                }
            }
        }        
    }
}
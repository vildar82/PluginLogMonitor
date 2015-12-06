using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace LogMonitor.Core.AllUsers
{
   public static class ADUtils
   {
      public static List<UserInfo> GetUsersInGroup(string groupName)
      {
         string domainName = "picompany.ru";
         //string groupName = "adm-dsk3-AutoCADSettings-u";
         List<UserInfo> users = new List<UserInfo>();
         using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName))
         {
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
         }
         return users;
      }

      private static void IterateGroup(GroupPrincipal group, HashSet<UserInfo> usersHash)
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
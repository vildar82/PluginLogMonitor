using System;

namespace LogMonitor.Core.AllUsers
{
    public class UserInfo : IComparable<UserInfo>
    {
        public string GroupAcad = string.Empty;
        public string GroupAD = string.Empty;
        public string LastError = string.Empty;
        public DateTime LastSuccesSetting = DateTime.MinValue;
        public string Login = string.Empty;
        public string Name = string.Empty;
        public Version NetVersion;

      public UserInfo(string name, string login, string group)
        {
            Name = name;
            Login = login;
            GroupAD = group;
        }

        public int CompareTo(UserInfo other)
        {
            return Login.CompareTo(other.Login);
        }

        public override bool Equals(object obj)
        {
            UserInfo u = obj as UserInfo;
            return u != null && u.Login == Login;
        }

        public override int GetHashCode()
        {
            return Login.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Имя: {0}; Логин: {1}; ГруппаAD: {2}; ГруппаAcad: {3}; LastSuccesSetting: {4}; LastError {5}"
               , Name, Login, GroupAD, GroupAcad, LastSuccesSetting, LastError);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LogMonitor.Core.AllUsers
{
    public class UserInfo : IComparable<UserInfo>, IEquatable<UserInfo>
    {
        public string GroupAcad = string.Empty;
        public string GroupAD = string.Empty;
        public string LastError = string.Empty;
        public DateTime LastSuccesSetting = DateTime.MinValue;
        public string Login = string.Empty;
        public string Name = string.Empty;
        public Version NetVersion;
        public Version AcadVersion;

        public UserInfo(string name, string login, string group)
        {
            Name = name;
            Login = login;
            GroupAD = group;
        }

        public string Department { get; set; }
        public string Position { get; set; }
        public List<string> Groups { get; set; }

        public bool Equals(UserInfo other)
        {
            return Login.Equals(other.Login, StringComparison.OrdinalIgnoreCase);
        }

        public int CompareTo([NotNull] UserInfo other)
        {
            return Login.CompareTo(other.Login);
        }

        public override bool Equals(object obj)
        {
            return obj is UserInfo u && u.Login == Login;
        }

        public override int GetHashCode()
        {
            return Login.GetHashCode();
        }

        public override string ToString()
        {
            return
	            $"Имя: {Name}; Логин: {Login}; ГруппаAD: {GroupAD}; ГруппаAcad: {GroupAcad}; LastSuccesSetting: {LastSuccesSetting}; LastError {LastError}";
        }        
    }
}
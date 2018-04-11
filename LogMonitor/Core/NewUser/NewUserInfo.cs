using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMonitor.Core.AllUsers;

namespace LogMonitor.Core.NewUser
{
    /// <summary>
    /// Инфо по новому пользователю
    /// </summary>
    public class NewUserInfo
    {
        public string UserName { get; set; }
        /// <summary>
        /// Рабочая группа (АР, КР-МН и т.п.)
        /// </summary>
        public string WorkGroup { get; set; }
        public string FIO { get; internal set; }
        public List<string> WorkGroups { get; internal set; }
        public string Department { get; set; }
        public string Position { get; set; }

        public NewUserInfo (string userName)
        {
            UserName = userName;
        }

        public override string ToString ()
        {
            return $"FIO={FIO}; UserName={UserName}; Workgroup={WorkGroup}; Position{Position}; Department={Department}; WorkGroups={string.Join(",", WorkGroups)}";
        }
    }
}

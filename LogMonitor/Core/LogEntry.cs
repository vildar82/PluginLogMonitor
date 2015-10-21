using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonitor.Core
{
   public class LogEntry
   {
      public string UserName { get; }
      public string Logs { get; set; }

      public LogEntry(string filename)
      {
         UserName = getUsernameFromLogFileName(filename);
      }

      private string getUsernameFromLogFileName(string filename)
      {
         return filename;
      }
   }
}

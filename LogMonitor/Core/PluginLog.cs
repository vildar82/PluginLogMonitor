using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMonitor.Core
{
   // Лог плагина
   public class PluginLog
   {
      public string PluginName { get; set; }      
      public Dictionary<string, LogEntry> Logs { get; set; }      
      
      public PluginLog(string name)
      {
         PluginName = name;
         Logs = new Dictionary<string, LogEntry>();
      }
   }
}

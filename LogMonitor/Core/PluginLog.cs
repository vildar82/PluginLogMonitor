using System.Collections.Generic;

namespace LogMonitor.Core
{
   // Лог плагина
   public class PluginLog
   {
      public PluginLog(string name)
      {
         PluginName = name;
         Logs = new Dictionary<string, LogEntry>();
      }

      public Dictionary<string, LogEntry> Logs { get; set; }
      public string PluginName { get; set; }
   }
}
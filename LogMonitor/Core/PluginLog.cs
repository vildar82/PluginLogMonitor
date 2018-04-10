using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LogMonitor.Core
{
   // Лог плагина
   public class PluginLog
   {
      public PluginLog(string name)
      {
         PluginName = name;
         Logs = new ConcurrentDictionary<string, LogEntry>();
      }

      public ConcurrentDictionary<string, LogEntry> Logs { get; set; }
      public string PluginName { get; set; }
   }
}
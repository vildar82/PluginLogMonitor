using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LogMonitor.Core
{
   public class Presenter
   {
      private Dictionary<string, PluginLog> _pluginsLog;

      public Presenter(Dictionary<string, PluginLog> pluginsLog)
      {
         _pluginsLog = pluginsLog;
      }

      public string GetBody()
      {
         var sbBody = new StringBuilder();
         foreach (var pluginLog in _pluginsLog)
         {
            sbBody.AppendLine($"{pluginLog.Value.PluginName}");
            foreach (var logEntry in pluginLog.Value.Logs)
            {
               sbBody.AppendLine(" ");
               sbBody.AppendLine($"\n{logEntry.Value.UserName}");
               sbBody.AppendLine(" ");
               sbBody.AppendLine($"{logEntry.Value.Logs}");
            }
         }
         return sbBody.ToString();
      }

      public DataTable GetDataTable()
      {
         var table = new DataTable("Отчет лог мониторинга");

         var colUserName = table.Columns.Add("Usernane");
         var colLog = table.Columns.Add("Log");

         var row = table.NewRow();
         row.SetField(colUserName, "Отчет логов плагинов");

         foreach (var pluginLog in _pluginsLog)
         {
            row = table.NewRow();
            row.SetField(colUserName, pluginLog.Key);

            foreach (var logEntry in pluginLog.Value.Logs)
            {
               row = table.NewRow();
               row.SetField(colUserName, logEntry.Value.UserName);
               row.SetField(colLog, logEntry.Value.Logs);
            }
         }
         return table;
      }

      public void SaveReport(string body)
      {
         var date = DateTime.Now.ToString();
         foreach (var c in System.IO.Path.GetInvalidFileNameChars())
         {
            date = date.Replace(c, '.');
         }

         var fileName = Path.Combine(LogService.LocalSavePath, $"PluginLogs-{date}.{"txt"}");

         try
         {
            File.WriteAllText(fileName, body);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
      }
   }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
         StringBuilder sbBody = new StringBuilder();
         foreach (var pluginLog in _pluginsLog)
         {
            sbBody.AppendLine();
            sbBody.AppendLine(string.Format("{0}", pluginLog.Value.PluginName));
            sbBody.AppendLine();
            foreach (var logEntry in pluginLog.Value.Logs)
            {
               sbBody.AppendLine(string.Format("\n{0}", logEntry.Value.UserName));               
               sbBody.AppendLine(string.Format("{0}", logEntry.Value.Logs));               
            }
         }
         return sbBody.ToString(); 
      }

      public DataTable GetDataTable()
      {
         DataTable table = new DataTable("Отчет лог мониторинга");

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
         string date = DateTime.Now.ToString();
         foreach (char c in System.IO.Path.GetInvalidFileNameChars())
         {
            date = date.Replace(c, '.');
         }

         string fileName = Path.Combine(LogService.LocalSavePath, string.Format("PluginLogs-{0}.{1}", date, "txt"));
         
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

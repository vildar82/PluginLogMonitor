using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using LogMonitor.Core.AllUsers;

namespace LogMonitor.Core
{
   // Следит за логами. собирает логи плагинов. отправляет сводку
   public class LogService
   {
      private static string _localSavePath = @"c:\work\test\Logs";
      private string _body;
      private DateTime _lastScan;
      private string _logPath = @"\\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs";
      private System.Timers.Timer _timer;

      public LogService()
      {
         FrequencyScanLogHours = 3; // сканировать логи каждые три часа.
         _lastScan = DateTime.Now.AddDays(-1);
      }

      public static string LocalSavePath { get { return _localSavePath; } }
      public string Body { get { return _body; } }
      public int FrequencyScanLogHours { get; set; }
      public DateTime LastScan { get { return _lastScan; } }

      public void Start()
      {
         setTimer();
         Timer_Elapsed(null, null);
      }

      public void Stop()
      {
         _timer.Dispose();
      }

      private Dictionary<string, PluginLog> getPluginsLog()
      {
         var dirLog = new DirectoryInfo(_logPath);
         var filesLog = dirLog.GetFiles("*.log");
         LogAnalizer logAnalizer = new LogAnalizer(_lastScan);
         foreach (var file in filesLog)
         {
            var logLines = File.ReadAllLines(file.FullName, Encoding.Default);
            logAnalizer.AnalisLogLines(logLines, Path.GetFileNameWithoutExtension(file.Name));
         }
         return logAnalizer.PluginsLog;
      }

      private void scanLogsAndSendReport()
      {
         Dictionary<string, PluginLog> pluginsLog = getPluginsLog();
         Presenter present = new Presenter(pluginsLog);
         _body = present.GetBody();
         if (!string.IsNullOrEmpty(_body))
         {
            EmailLog.SendEmail(_body, "Plugin Log Monitor");
         }
         present.SaveReport(_body);
      }

      private void setTimer()
      {
         _timer = new System.Timers.Timer(FrequencyScanLogHours * 3600000);
         _timer.AutoReset = true;
         _timer.Enabled = true;
         _timer.Elapsed += Timer_Elapsed;
      }

      private void Timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         try
         {
            scanLogsAndSendReport();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.ToString());
         }
         _lastScan = DateTime.Now;
      }
   }
}
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
	    private string _logPath = @"\\dsk2.picompany.ru\project\CAD_Settings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs";
        private System.Timers.Timer _timer;

        public LogService ()
        {
            FrequencyScanLogHours = 3; // сканировать логи каждые три часа.
            LastScan = DateTime.Now.AddDays(-1);
        }

        public static string LocalSavePath { get; } = @"c:\temp\Logs";

	    public string Body { get; private set; }
	    public int FrequencyScanLogHours { get; set; }
        public DateTime LastScan { get; private set; }

	    public void Start ()
        {
            setTimer();
            Timer_Elapsed(null, null);
        }

        public void Stop ()
        {
            _timer.Dispose();
        }

        private Dictionary<string, PluginLog> getPluginsLog ()
        {
            var dirLog = new DirectoryInfo(_logPath);
            var filesLog = dirLog.GetFiles("*.log");
            var logAnalizer = new LogAnalizer(LastScan);
            foreach (var file in filesLog)
            {
                var logLines = File.ReadAllLines(file.FullName, Encoding.Default);
                logAnalizer.AnalisLogLines(logLines, Path.GetFileNameWithoutExtension(file.Name));
            }
            return logAnalizer.PluginsLog;
        }

        private void scanLogsAndSendReport ()
        {
            var pluginsLog = getPluginsLog();
            var present = new Presenter(pluginsLog);
            Body = present.GetBody();
            if (!string.IsNullOrEmpty(Body))
            {
                EmailLog.SendEmail(Body, "Plugin Log Monitor");
            }
            present.SaveReport(Body);
        }

        private void setTimer ()
        {
            _timer = new System.Timers.Timer(FrequencyScanLogHours * 3600000);
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed (object sender, ElapsedEventArgs e)
        {
            try
            {
                scanLogsAndSendReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LastScan = DateTime.Now;
        }
    }
}
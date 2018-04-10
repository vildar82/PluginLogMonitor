using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using JetBrains.Annotations;
using LogMonitor.Core.AllUsers;

namespace LogMonitor.Core
{
    // Следит за логами. собирает логи плагинов. отправляет сводку
    public class LogService
    {
	    public const string _logPath = @"\\picompany.ru\pikp\lib\_CadSettings\AutoCAD_server\ShareSettings\AutoCAD_PIK_Manager\Logs";
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
            SetTimer();
            Timer_Elapsed(null, null);
        }

        public void Stop ()
        {
            _timer.Dispose();
        }

        private ConcurrentDictionary<string, PluginLog> GetPluginsLog ()
        {
            var dirLog = new DirectoryInfo(_logPath);
            var filesLog = dirLog.GetFiles("*.log");
            Parallel.ForEach(filesLog, l =>
            {
                var logAnalizer = new LogAnalizer(LastScan);
                var logLines = File.ReadAllLines(l.FullName, Encoding.Default);
                logAnalizer.AnalisLogLines(logLines, Path.GetFileNameWithoutExtension(l.Name));
            });
            return LogAnalizer.PluginsLog;
        }

        private void ScanLogsAndSendReport ()
        {
            var pluginsLog = GetPluginsLog();
            var present = new Presenter(pluginsLog);
            Body = present.GetBody();
            if (!string.IsNullOrEmpty(Body))
            {
                EmailLog.SendEmail(Body, "Plugin Log Monitor");
            }
            present.SaveReport(Body);
        }

        private void SetTimer ()
        {
	        _timer = new System.Timers.Timer(FrequencyScanLogHours * 3600000)
	        {
		        AutoReset = true,
		        Enabled = true
	        };
	        _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed (object sender, ElapsedEventArgs e)
        {
            try
            {
                ScanLogsAndSendReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LastScan = DateTime.Now;
        }

	    [NotNull]
	    public static string GetUserLogs(string user)
	    {
		    var userLogFiles = new DirectoryInfo(_logPath).EnumerateFiles($"{user}*", SearchOption.TopDirectoryOnly)
			    .OrderBy(o => o.LastWriteTime).Select(s => File.ReadAllText(s.FullName, Encoding.Default));
		    return string.Join(Environment.NewLine, userLogFiles);
	    }
    }
}
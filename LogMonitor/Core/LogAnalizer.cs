using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LogMonitor.Core
{
    public class LogAnalizer
    {
        private bool _lastLogIsPlugin;
        private readonly DateTime _lastScan;
        private LogEntry _logEntry;
        private PluginLog _pluginLog;
        private string _username;

        public LogAnalizer (DateTime lastScan)
        {
            _lastScan = lastScan;
        }

        public static ConcurrentDictionary<string, PluginLog> PluginsLog { get; } = new ConcurrentDictionary<string, PluginLog>();

        public void AnalisLogLines ([NotNull] IEnumerable<string> logLines, string filename)
        {
            _pluginLog = null;
            _lastLogIsPlugin = false;
            _logEntry = null;
            _username = filename;
            foreach (var line in logLines)
            {
                ParsingLine(line);
            }
        }

        private void ParsingLine ([NotNull] string line)
        {
            if (LineIsLog(line))
            {
                // проверка даты лога (сравнение с последней датой сбора логов)
                if (CheckLogDate(line))
                {
                    var msgLine = GetLineMsg(line);
                    if (msgLine.StartsWith("Plugin "))
                    {
                        msgLine = msgLine.Substring(7);
                        string pluginName;
                        try
                        {
                            pluginName = msgLine.Substring(0, msgLine.IndexOf(' '));
                            msgLine = msgLine.Substring(pluginName.Length + 1);
                        }
                        catch
                        {
                            pluginName = msgLine;
                        }
                        if (!PluginsLog.TryGetValue(pluginName, out _pluginLog))
                        {
                            _pluginLog = new PluginLog(pluginName);
                            PluginsLog.TryAdd(_pluginLog.PluginName, _pluginLog);
                        }
                        if (!_pluginLog.Logs.TryGetValue(_username, out _logEntry))
                        {
                            _logEntry = new LogEntry(_username);
                            _pluginLog.Logs.TryAdd(_username, _logEntry);
                        }
                        _logEntry.Logs += $"\n{line}";
                        _lastLogIsPlugin = true;
                    }
                    else
                    {
                        _lastLogIsPlugin = false;
                        // Проверка новых пользователей                        
                        NewUser.NewUserService.CheckNewUserInMsgLine(msgLine, _username.Substring(0, _username.IndexOf("-")));
                    }
                }
            }
            else
            {
                if (_lastLogIsPlugin)
                {
                    _logEntry.Logs += $"\n{line}";
                }
            }
        }

        private static bool LineIsLog ([NotNull] string line)
        {
	        if (line.Length <= 25) return false;
	        var startDateTimeLine = line.Substring(0, 24);
	        if (DateTime.TryParse(startDateTimeLine, out DateTime _))
	        {
		        return true;
	        }
	        return false;
        }

        private bool CheckLogDate ([NotNull] string line)
        {
            var dateIndex = line.IndexOf('_');
            if (dateIndex == -1) dateIndex = line.IndexOf('|');
            if (dateIndex ==-1) throw new InvalidOperationException();
            var dateText = line.Substring(0, dateIndex);
			if (DateTime.TryParse(dateText, out var logTime))
            {
                if (logTime > _lastScan)
                {
                    return true;
                }
            }
            return false;
        }

        [NotNull]
        private static string GetLineMsg ([NotNull] string line)
        {
            // из строки вида:
            // 2015-09-17 21:28:31.5454_Info:  Версия автокада - 20.1.0.0
            // вернуть
            // Версия автокада - 20.1.0.0
            return line.Substring(line.IndexOf("  ") + 2); //Версия автокада - 20.1.0.0
        }
    }
}
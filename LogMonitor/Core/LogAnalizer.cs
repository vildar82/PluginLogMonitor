using System;
using System.Collections.Generic;

namespace LogMonitor.Core
{
    public class LogAnalizer
    {
        private bool _lastLogIsPlugin;
        private readonly DateTime _lastScan;
        private LogEntry _logEntry;
        private PluginLog _pluginLog;
        private readonly Dictionary<string, PluginLog> _pluginsLog = new Dictionary<string, PluginLog>();
        private string _username;

        public LogAnalizer (DateTime lastScan)
        {
            _lastScan = lastScan;
        }

        public Dictionary<string, PluginLog> PluginsLog => _pluginsLog;

	    public void AnalisLogLines (IEnumerable<string> logLines, string filename)
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

        private void ParsingLine (string line)
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
                        if (!_pluginsLog.TryGetValue(pluginName, out _pluginLog))
                        {
                            _pluginLog = new PluginLog(pluginName);
                            _pluginsLog.Add(_pluginLog.PluginName, _pluginLog);
                        }
                        if (!_pluginLog.Logs.TryGetValue(_username, out _logEntry))
                        {
                            _logEntry = new LogEntry(_username);
                            _pluginLog.Logs.Add(_username, _logEntry);
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

        private static bool LineIsLog (string line)
        {
	        if (line.Length <= 25) return false;
	        var startDateTimeLine = line.Substring(0, 24);
	        if (DateTime.TryParse(startDateTimeLine, out DateTime _))
	        {
		        return true;
	        }
	        return false;
        }

        private bool CheckLogDate (string line)
        {
			var dateText = line.Substring(0, line.IndexOf('_'));
			if (DateTime.TryParse(dateText, out DateTime logTime))
            {
                if (logTime > _lastScan)
                {
                    return true;
                }
            }
            return false;
        }

        private static string GetLineMsg (string line)
        {
            // из строки вида:
            // 2015-09-17 21:28:31.5454_Info:  Версия автокада - 20.1.0.0
            // вернуть
            // Версия автокада - 20.1.0.0
            return line.Substring(line.IndexOf("  ") + 2); //Версия автокада - 20.1.0.0
        }
    }
}
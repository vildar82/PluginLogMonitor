namespace LogMonitor.Core
{
   public class LogEntry
   {
      public LogEntry(string filename)
      {
         UserName = getUsernameFromLogFileName(filename);
      }

      public string Logs { get; set; }
      public string UserName { get; }

      private string getUsernameFromLogFileName(string filename)
      {
         return filename;
      }
   }
}
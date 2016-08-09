using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ProcessRunner
{
   class Log
   {
      private const String LOG_NAME = "ProcessRunner";
      private const String LOG_TYPE = "Application";

      private static string _logDirectory;
      public static String LogDirectory
      {
         set
         {
            String stNewDirectory = value;
            if (!stNewDirectory.EndsWith(Char.ToString(Path.DirectorySeparatorChar)))
               stNewDirectory += Path.DirectorySeparatorChar;

            _logDirectory = stNewDirectory;
         }
      }

      public static void logLn(String stMessage)
      {
         if (!EventLog.SourceExists(LOG_NAME))
            EventLog.CreateEventSource(LOG_NAME, LOG_TYPE);

         EventLog.WriteEntry(LOG_NAME, stMessage);
      }
   }
}

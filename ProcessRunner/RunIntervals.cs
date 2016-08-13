using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessRunner
{
   class RunIntervals
   {
      public const int FIRST_RUN = -1;
      private const String CFG_RUN_INTERVAL = "RunInterval";
      private const Char TIME_RANGE_DELIMIT = '-';
      private const Char TIME_DELIMIT = ':';
      private const Char INTERVAL_DELIMIT = ',';

      private LinkedList<RunInterval> _runIntervals;

      public RunIntervals(Config cfg)
      {
         _runIntervals = new LinkedList<RunInterval>();

         foreach (String cfgKey in cfg.getCfgKeys())
         {
            if (cfgKey.StartsWith(CFG_RUN_INTERVAL))
               _runIntervals.AddLast(parseCfgLine(cfg.getCfgVal(cfgKey)));
         }
      }

      private RunInterval parseCfgLine(String stCfgLine)
      {
         int idxTimeDelimit = stCfgLine.IndexOf(TIME_RANGE_DELIMIT);
         int idIntervalDelimit = stCfgLine.IndexOf(INTERVAL_DELIMIT);

         String[] aStStart = stCfgLine.Substring(0, idxTimeDelimit).Split(TIME_DELIMIT);
         TimeSpan startTime = new TimeSpan(Int32.Parse(aStStart[0]), Int32.Parse(aStStart[1]), 0);

         String[] aStEnd = stCfgLine.Substring(idxTimeDelimit + 1, idIntervalDelimit - idxTimeDelimit - 1).Split(TIME_DELIMIT);
         TimeSpan endTime = new TimeSpan(Int32.Parse(aStEnd[0]), Int32.Parse(aStEnd[1]), 0);

         int nIntervalMin = Int32.Parse(stCfgLine.Substring(idIntervalDelimit + 1));

         RunInterval newRunInterval = new RunInterval(startTime, endTime, nIntervalMin);
         Log.logLn(String.Format("Parsed run interval: {0}", newRunInterval.toString()));

         return newRunInterval;
      }

      public bool runNow(int minutesWaited)
      {
         if (minutesWaited == FIRST_RUN)
            return true;

         TimeSpan currentTime = DateTime.Now.TimeOfDay;
         foreach (RunInterval runInterval in _runIntervals)
         {
            bool? bRunNow = runInterval.runNow(currentTime, minutesWaited);
            if (bRunNow != null)
               return (bool)bRunNow;
         }

         return false;
      }

      private class RunInterval
      {
         private TimeSpan _startTime, _endTime;
         private int _intervalMin;

         public RunInterval(TimeSpan startTime, TimeSpan endTime, int intervalMin)
         {
            _startTime = startTime;
            _endTime = endTime;
            _intervalMin = intervalMin;
         }

         public bool? runNow(TimeSpan currentTime, int minutesWaited)
         {
            if (currentTime < _startTime || currentTime > _endTime)
               return null;

            return minutesWaited >= _intervalMin;
         }

         public String toString()
         {
            return String.Format("Start: {0}, End: {1}, Interval (min): {2}", _startTime, _endTime, _intervalMin);
         }
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessRunner
{
   class FrequencyRanges
   {
      public const int FIRST_RUN = -1;
      private const String CFG_FREQUENCY_RANGE = "FrequencyRange";
      private const Char RANGE_DELIMIT = '-';
      private const Char TIME_DELIMIT = ':';
      private const Char FREQUENCY_DELIMIT = ',';

      private LinkedList<FrequencyRange> _frequencyRanges;

      public FrequencyRanges(Config cfg)
      {
         _frequencyRanges = new LinkedList<FrequencyRange>();

         foreach (String cfgKey in cfg.getCfgKeys())
         {
            if (cfgKey.StartsWith(CFG_FREQUENCY_RANGE))
               _frequencyRanges.AddLast(parseCfgLine(cfg.getCfgVal(cfgKey)));
         }
      }

      private FrequencyRange parseCfgLine(String stCfgLine)
      {
         int idxTimeDelimit = stCfgLine.IndexOf(RANGE_DELIMIT);
         int idxFrequencyDelimit = stCfgLine.IndexOf(FREQUENCY_DELIMIT);

         String[] aStStart = stCfgLine.Substring(0, idxTimeDelimit).Split(TIME_DELIMIT);
         TimeSpan startTime = new TimeSpan(Int32.Parse(aStStart[0]), Int32.Parse(aStStart[1]), 0);

         String[] aStEnd = stCfgLine.Substring(idxTimeDelimit + 1, idxFrequencyDelimit - idxTimeDelimit - 1).Split(TIME_DELIMIT);
         TimeSpan endTime = new TimeSpan(Int32.Parse(aStEnd[0]), Int32.Parse(aStEnd[1]), 0);

         int nFrequency = Int32.Parse(stCfgLine.Substring(idxFrequencyDelimit + 1));

         FrequencyRange newFrequencyRange = new FrequencyRange(startTime, endTime, nFrequency);
         Log.logLn(String.Format("Parsed frequency range: {0}", newFrequencyRange.toString()));

         return newFrequencyRange;
      }

      public bool runNow(int minutesWaited)
      {
         if (minutesWaited == FIRST_RUN)
            return true;

         TimeSpan currentTime = DateTime.Now.TimeOfDay;
         foreach (FrequencyRange frequencyRange in _frequencyRanges)
         {
            bool? bRunNow = frequencyRange.runNow(currentTime, minutesWaited);
            if (bRunNow != null)
               return (bool)bRunNow;
         }

         return false;
      }

      private class FrequencyRange
      {
         private TimeSpan _startTime, _endTime;
         private int _frequency;

         public FrequencyRange(TimeSpan startTime, TimeSpan endTime, int frequency)
         {
            _startTime = startTime;
            _endTime = endTime;
            _frequency = frequency;
         }

         public bool? runNow(TimeSpan currentTime, int minutesWaited)
         {
            if (currentTime < _startTime || currentTime > _endTime)
               return null;

            return minutesWaited >= _frequency;
         }

         public String toString()
         {
            return String.Format("Start: {0}, End: {1}, Frequency (min): {2}", _startTime, _endTime, _frequency);
         }
      }
   }
}

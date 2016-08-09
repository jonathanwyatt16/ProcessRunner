using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProcessRunner
{
   class Config
   {
      private const Char CFG_PREFIX = '#';
      private const Char CFG_DELIMIT = '=';
      private const String CFG_EXTENSION = ".cfg";
      private static String _configDirectory;

      private String _cfgPath;
      private Dictionary<string, string> _cfgVals;

      public Config(String stCfgName)
      {
         _cfgPath = ConfigDirectory + stCfgName + CFG_EXTENSION;
         _cfgVals = new Dictionary<string, string>();

         loadConfig();
      }

      private void loadConfig()
      {
         Log.logLn("Loading config " + _cfgPath);
         StreamReader streamReader = new StreamReader(_cfgPath);
         String stLine;

         while ((stLine = streamReader.ReadLine()) != null)
         {
            if (!stLine.StartsWith(Char.ToString(CFG_PREFIX)))
               continue;

            String[] aStCfg = stLine.Split(CFG_DELIMIT);

            String stKey = aStCfg[0].Substring(1);
            String stConfig = aStCfg[1];
            Log.logLn(String.Format("Loaded cfg key = {0}, cfg value = {1}", stKey, stConfig));

            _cfgVals.Add(stKey, stConfig);
         }
      }

      public String getCfgVal(String stCfgKey)
      {
         return _cfgVals[stCfgKey];
      }

      public List<String> getCfgKeys()
      {
         return new List<String>(_cfgVals.Keys);
      }

      public static String ConfigDirectory
      {
         get
         {
            return _configDirectory;
         }
         set
         {
            String stNewDirectory = value;
            if (!stNewDirectory.EndsWith(Char.ToString(Path.DirectorySeparatorChar)))
               stNewDirectory += Path.DirectorySeparatorChar;

            _configDirectory = stNewDirectory;
            Log.logLn("Set config directory = " + _configDirectory);
         }
      }
   }
}

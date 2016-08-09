using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace ProcessRunner
{
   class ProcessRunnerService : ServiceBase
   {

      public enum ServiceState
      {
         SERVICE_STOPPED = 0x00000001,
         SERVICE_START_PENDING = 0x00000002,
         SERVICE_STOP_PENDING = 0x00000003,
         SERVICE_RUNNING = 0x00000004,
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct ServiceStatus
      {
         public long dwServiceType;
         public ServiceState dwCurrentState;
         public long dwControlsAccepted;
         public long dwWin32ExitCode;
         public long dwServiceSpecificExitCode;
         public long dwCheckPoint;
         public long dwWaitHint;
      };

      [DllImport("advapi32.dll", SetLastError = true)]
      private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

      private const String CFG_PROCESS_DIRECTORY = "ProcessWorkingDirectory";
      private const String CFG_PROCESS_NAME = "ProcessName";
      private const String CFG_NAME = "ProcessRunner";
      private const String CFG_DIRECTORY = "config";
      private const String BIN_DIRECTORY = "bin";
      
      private ServiceStatus _currentStatus;
      private Process _process;
      private FrequencyRanges _frequencyRanges;
      private int _minutesWaited;

      protected override void OnStart(string[] aSt)
      {
         Log.logLn("Starting ProcessRunnerService.");

         Config.ConfigDirectory = AppDomain.CurrentDomain.BaseDirectory.Replace(BIN_DIRECTORY, CFG_DIRECTORY); ;
         loadCfg(new Config(CFG_NAME));

         Thread processRunner = new Thread(runProcessRunnerService);
         processRunner.Start();

         _currentStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
         SetServiceStatus(this.ServiceHandle, ref _currentStatus);
      }

      private void loadCfg(Config cfg)
      {
         _currentStatus = new ServiceStatus();
         _currentStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
         _currentStatus.dwWaitHint = 100000;
         SetServiceStatus(this.ServiceHandle, ref _currentStatus);

         String stWorkingDirectory = cfg.getCfgVal(CFG_PROCESS_DIRECTORY);
         if (!stWorkingDirectory.EndsWith(Char.ToString(Path.DirectorySeparatorChar)))
            stWorkingDirectory += Path.DirectorySeparatorChar;

         String stProcessName = cfg.getCfgVal(CFG_PROCESS_NAME);

         ProcessStartInfo processInfo = new ProcessStartInfo();
         processInfo.WorkingDirectory = stWorkingDirectory;
         processInfo.FileName = stWorkingDirectory + stProcessName;
         processInfo.WindowStyle = ProcessWindowStyle.Hidden;

         _process = new Process();
         _process.StartInfo = processInfo;

         _frequencyRanges = new FrequencyRanges(cfg);
         _minutesWaited = FrequencyRanges.FIRST_RUN;
      }

      private void runProcessRunnerService()
      {
         while (true)
         {
            if (_frequencyRanges.runNow(_minutesWaited))
            {
               Log.logLn(String.Format("Running process {0}.", _process.StartInfo.FileName));
               _process.Start();
               _process.WaitForExit();
               _process.Close();
               _minutesWaited = 0;
               Log.logLn(String.Format("Done running process {0}.", _process.StartInfo.FileName));
            }

            else
            {
               Thread.Sleep(60000);
               _minutesWaited += 1;
            }
         }
      }

      protected override void OnStop()
      {
         Log.logLn("Stopping process");
         _currentStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
         SetServiceStatus(this.ServiceHandle, ref _currentStatus);

         _process.Close();

         _currentStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
         SetServiceStatus(this.ServiceHandle, ref _currentStatus);
      }

      private void InitializeComponent()
      {
         this.ServiceName = "ProcessRunnerService";
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ProcessRunner
{
   static class ProcessRunnerMain
   {
      static void Main()
      {
         ServiceBase[] ServicesToRun;

         ServicesToRun = new ServiceBase[] 
			{ 
				new ProcessRunnerService() 
			};
         
         ServiceBase.Run(ServicesToRun);
      }
   }
}

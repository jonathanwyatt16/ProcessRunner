namespace ProcessRunner
{
   partial class ProjectInstaller
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.ProcessRunnerInstaller = new System.ServiceProcess.ServiceProcessInstaller();
         this.ProcessRunnerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
         // 
         // ProcessRunnerInstaller
         // 
         this.ProcessRunnerInstaller.Password = null;
         this.ProcessRunnerInstaller.Username = null;
         // 
         // ProcessRunnerServiceInstaller
         // 
         this.ProcessRunnerServiceInstaller.Description = "ProcessRunner runs a specified process at varying frequencies throughout the day." +
             "";
         this.ProcessRunnerServiceInstaller.DisplayName = "ProcessRunnerService";
         this.ProcessRunnerServiceInstaller.ServiceName = "ProcessRunnerService";
         // 
         // ProjectInstaller
         // 
         this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProcessRunnerInstaller,
            this.ProcessRunnerServiceInstaller});

      }

      #endregion

      private System.ServiceProcess.ServiceProcessInstaller ProcessRunnerInstaller;
      private System.ServiceProcess.ServiceInstaller ProcessRunnerServiceInstaller;
   }
}
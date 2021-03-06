﻿using AMaungUs.FFUMaker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class CreateTestImageViewModel : BaseViewModel
    {
        PrerequisiteViewModel prerequisites;

        public Workspace ws;

        string productName;
        public string ProductName
        {
            get { return productName; }
            set { SetProperty(ref productName, value); }
        }
        string log;
        public string Log
        {
            get { return log; }
            set { SetProperty(ref log, value); }
        }

        bool executingPowershell = false;
        public bool ExecutingPowershell
        {
            get { return executingPowershell; }
            set { SetProperty(ref executingPowershell, value); }
        }
        BackgroundWorker bkgWorker;
        public event EventHandler Create;
        public CreateTestImageViewModel()
        {
            ws = new Workspace();
            prerequisites = new PrerequisiteViewModel();
            bkgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true

            };
            bkgWorker.DoWork += BkgWorker_DoWork; ;
            bkgWorker.RunWorkerCompleted += BkgWorker_RunWorkerCompleted;
        }

        System.Windows.Input.ICommand createCommand;
        public ICommand CreateCommand
        {
            get { return createCommand == null ? new DelegateCommand<object>(this.CreateCommandExec, x => true) : CreateCommand; }
            set { SetProperty(ref createCommand, value); }
        }
        private void CreateCommandExec(object parm)
        {
            var validateResult = ValidateWorkspace();
            if (validateResult && !bkgWorker.IsBusy)
            {
                ExecutingPowershell = true;
                bkgWorker.RunWorkerAsync();
            }

        }

        private void BkgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Create(null, new EventArgs());
        }

        private void BkgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RunPowershellScripts();
            e.Cancel = true;
        }
        private bool ValidateWorkspace()
        {
            if (string.IsNullOrEmpty(ProductName))
                return false;
            else
            {
                return true;
            }
        }
        private void RunPowershellScripts()
        {
            Task.Delay(1000);
            string file = prerequisites.AdkAddOnKitPath + "\\Tools\\LaunchShell.ps1";

            FileInfo fInfo = new FileInfo(file);
            var f = fInfo.DirectoryName;
            string newFilePath = f + @"\LaunchshellMod.ps1";
            string commands = string.Empty;
            using (StreamReader sr = new StreamReader(file))
            {
                commands = sr.ReadToEnd();
            }
            commands += "\n" + "$openworkspacecmd = 'open-ws " + ws.Path + "\\" + ws.Name + "'";
            commands += "\ninvoke-expression  $openworkspacecmd";
            commands += "\n" + "$createTestImgCmd = 'New-IoTFFUImage " + ProductName + " Test'";
            commands += "\ninvoke-expression  $createTestImgCmd";
            using (StreamWriter sw = new StreamWriter(newFilePath))
            {
                sw.WriteLine(commands);
            }
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe ";
            psi.Arguments = @" -noexit -ExecutionPolicy Bypass -Command " + newFilePath + " -Verb runAs";
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            p.StartInfo = psi;
            p.Start();
            p.StandardInput.WriteLine("Exit");
            p.WaitForExit();
            File.Delete(newFilePath);
        }
    }

}

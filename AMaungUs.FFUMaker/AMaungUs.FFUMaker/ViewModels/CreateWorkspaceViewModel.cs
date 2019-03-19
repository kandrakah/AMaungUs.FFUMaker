using AMaungUs.FFUMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.IO;
using System.Diagnostics;

namespace AMaungUs.FFUMaker.ViewModels
{
     public class CreateWorkspaceViewModel : BaseViewModel
    {

        string workspacePath;
        PrerequisiteViewModel prerequisites;
        public string WorkspacePath
        {
            get { return workspacePath; }
            set { SetProperty(ref workspacePath, value); }
        }
        string workspaceName;
        public string WorkspaceName
        {
            get { return workspaceName; }
            set { SetProperty(ref workspaceName, value); }
        }
        string oemName;
        public string OEMName
        {
            get { return oemName; }
            set { SetProperty(ref oemName, value); }
        }
        string architecture;
        public string Architecture
        {
            get { return architecture; }
            set { SetProperty(ref architecture, value); }
        }
        public List<String> ArchitectureList
        {
            get
            {
                var result = new List<string>();
                if (prerequisites.HasArmPackages)
                    result.Add("Arm");
                if (prerequisites.HasArm64Packages)
                    result.Add("Arm64");
                if (prerequisites.HasX86Packages)
                    result.Add("x86");
                if (prerequisites.HasX64Packages)
                    result.Add("x64");
                if(result.Count>0)
                    Architecture = result[0];
                return result;
            }
        }
        public Workspace workspace { get; set; }
        public event EventHandler Create;
        public CreateWorkspaceViewModel()
        {
            prerequisites = new PrerequisiteViewModel();
            OnPropertyChanged("ArchitectureList");
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
            if (validateResult)
            {
                RunPowershellScripts();
                this.Create(parm, new EventArgs());
            }
        }
        private bool ValidateWorkspace()
        {
            if (string.IsNullOrEmpty(WorkspaceName) || string.IsNullOrEmpty(OEMName) || string.IsNullOrEmpty(Architecture) || string.IsNullOrEmpty(WorkspacePath))
                return false;
            else
            {
                workspace = new Workspace() { Architecture = Architecture, OEMName = OEMName, Name = WorkspaceName, Path = WorkspacePath };
                return true;
            }
        }
        private void RunPowershellScripts()
        {
            string file = prerequisites.AdkAddOnKitPath+ "\\Tools\\LaunchShell.ps1";
            Directory.CreateDirectory(WorkspacePath + "\\" + WorkspaceName);
            FileInfo fInfo = new FileInfo(file);
            var f = fInfo.DirectoryName;
            string newFilePath = f + @"\LaunchshellMod.ps1";
            string commands = string.Empty;
            using (StreamReader sr = new StreamReader(file))
            {
                // and plunk the contents in the textbox
                commands = sr.ReadToEnd();
            }
            commands += "\n" + "$newCmd = 'new-ws " + WorkspacePath + "\\" + WorkspaceName + " " + OEMName + " " + Architecture +"'" + "\n";
            commands += "invoke-expression  $newCmd";
            using (StreamWriter sw = new StreamWriter(newFilePath))
            {
                sw.WriteLine(commands);
            }
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe ";
            //psi.Arguments = @" -noexit -ExecutionPolicy Bypass -Command C:\Dev\OpenSource\iot-adk-addonkit\Tools\Launchshell.ps1 -Verb runAs";
            psi.Arguments = @" -noexit -ExecutionPolicy Bypass -Command " + newFilePath + " -Verb runAs";
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            p.StartInfo = psi;
            p.Start();
            p.WaitForExit(2000);
            p.StandardInput.WriteLine("exit");
            File.Delete(newFilePath);
        }

        System.Windows.Input.ICommand pathSelectionCommand;
        public ICommand PathSelectionCommand
        {
            get { return pathSelectionCommand == null ? new DelegateCommand<object>(this.PathSelectionCommandExec, x => true) : pathSelectionCommand; }
            set { SetProperty(ref pathSelectionCommand, value); }
        }
        private void PathSelectionCommandExec(object parm)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (WorkspacePath != "Path not set.")
            {
                folderDialog.SelectedPath = WorkspacePath;
            }
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = folderDialog.SelectedPath;
                    WorkspacePath = folder;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
    }
}

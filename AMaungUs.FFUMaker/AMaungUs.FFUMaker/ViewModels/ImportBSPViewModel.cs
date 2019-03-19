using AMaungUs.FFUMaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class ImportBSPViewModel : BaseViewModel
    {

        PrerequisiteViewModel prerequisites;
        string location;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }
        string boardName;
        public string BoardName
        {
            get { return boardName; }
            set { SetProperty(ref boardName, value); }
        }
        string manufacturer;
        public string Manufacturer
        {
            get { return manufacturer; }
            set { SetProperty(ref manufacturer, value); }
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
                if (result.Count > 0)
                    Architecture = result[0];
                return result;
            }
        }
        public BSP BoardSupportPackage { get; set; }
        public event EventHandler Create;
        public ImportBSPViewModel()
        {
            prerequisites = new PrerequisiteViewModel();
            OnPropertyChanged("ArchitectureList");
        }
        System.Windows.Input.ICommand importCommand;
        public ICommand ImportCommand
        {
            get { return importCommand == null ? new DelegateCommand<object>(this.ImportCommandExec, x => true) : ImportCommand; }
            set { SetProperty(ref importCommand, value); }
        }
        private void ImportCommandExec(object parm)
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
            if (string.IsNullOrEmpty(BoardName) || string.IsNullOrEmpty(Manufacturer) || string.IsNullOrEmpty(Architecture) || string.IsNullOrEmpty(Location))
                return false;
            else
            {
                BoardSupportPackage = new BSP() { Architecture = Architecture, Manufacturer = Manufacturer, BoardName = BoardName, Location = Location };
                return true;
            }
        }
        private void RunPowershellScripts()
        {
            //string file = prerequisites.AdkAddOnKitPath + "\\Tools\\LaunchShell.ps1";
            //Directory.CreateDirectory(WorkspacePath + "\\" + WorkspaceName);
            //FileInfo fInfo = new FileInfo(file);
            //var f = fInfo.DirectoryName;
            //string newFilePath = f + @"\LaunchshellMod.ps1";
            //string commands = string.Empty;
            //using (StreamReader sr = new StreamReader(file))
            //{
            //    // and plunk the contents in the textbox
            //    commands = sr.ReadToEnd();
            //}
            //commands += "\n" + "$newCmd = 'new-ws " + WorkspacePath + "\\" + WorkspaceName + " " + OEMName + " " + Architecture + "'" + "\n";
            //commands += "invoke-expression  $newCmd";
            //using (StreamWriter sw = new StreamWriter(newFilePath))
            //{
            //    sw.WriteLine(commands);
            //}
            //Process p = new Process();
            //ProcessStartInfo psi = new ProcessStartInfo();
            //psi.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe ";
            ////psi.Arguments = @" -noexit -ExecutionPolicy Bypass -Command C:\Dev\OpenSource\iot-adk-addonkit\Tools\Launchshell.ps1 -Verb runAs";
            //psi.Arguments = @" -noexit -ExecutionPolicy Bypass -Command " + newFilePath + " -Verb runAs";
            //psi.RedirectStandardInput = true;
            //psi.RedirectStandardOutput = true;
            //psi.UseShellExecute = false;
            //psi.CreateNoWindow = true;
            //p.StartInfo = psi;
            //p.Start();
            //p.WaitForExit(2000);
            //p.StandardInput.WriteLine("exit");
            //File.Delete(newFilePath);
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
            if (Location != "Path not set.")
            {
                folderDialog.SelectedPath = Location;
            }
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = folderDialog.SelectedPath;
                    Location = folder;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
    }
}


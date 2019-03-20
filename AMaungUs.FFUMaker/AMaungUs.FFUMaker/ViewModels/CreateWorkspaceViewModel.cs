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
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Windows;
using System.Threading;
using System.ComponentModel;

namespace AMaungUs.FFUMaker.ViewModels
{
     public class CreateWorkspaceViewModel : BaseViewModel
    {
        string workspacePath;
        PrerequisiteViewModel prerequisites;
        private List<BSPManufacturerInfo> bspManufacturerDefinitions;
        public List<BSPManufacturerInfo> BSPManufacturerDefinitions
        {
            get { return bspManufacturerDefinitions == null ? new List<BSPManufacturerInfo>() : bspManufacturerDefinitions; }
            set { SetProperty(ref bspManufacturerDefinitions, value); }
        }
        public string WorkspacePath
        {
            get { return workspacePath; }
            set { SetProperty(ref workspacePath, value); }
        }
        string workspaceName;
        public string WorkspaceName
        {
            get { return workspaceName; }
            set
            {
                value = value.Replace(" ", "");
                SetProperty(ref workspaceName, value);
            }
        }
        string oemName;
        public string OEMName
        {
            get { return oemName; }
            set
            {
                value = value.Replace(" ", "");
                SetProperty(ref oemName, value);
            }
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
        string location;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }
        BSPManufacturerInfo manufacturer;
        public BSPManufacturerInfo SelectedManufacturer
        {
            get { return manufacturer == null ? new BSPManufacturerInfo() : manufacturer; }
            set { SetProperty(ref manufacturer, value); }
        }
        bool executingPowershell = false;
        public bool ExecutingPowershell
        {
            get { return executingPowershell; }
            set { SetProperty(ref executingPowershell, value); }
        }
        public event EventHandler Create;
        BackgroundWorker bkgWorker;
        public CreateWorkspaceViewModel()
        {
            prerequisites = new PrerequisiteViewModel();
            OnPropertyChanged("ArchitectureList");
            LoadJson();
            bkgWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true

            };
            bkgWorker.DoWork += BkgWorker_DoWork; ;
            bkgWorker.RunWorkerCompleted += BkgWorker_RunWorkerCompleted;
        }

        

        private void LoadJson()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader r = new StreamReader(path + "/Assets/BSPList.json"))
            {
                string json = r.ReadToEnd();
                BSPManufacturerDefinitions = JsonConvert.DeserializeObject<List<BSPManufacturerInfo>>(json);
            }
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

                ExecutingPowershell = true;
                bkgWorker.RunWorkerAsync();
                
            }
        }

            private void BkgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                    this.Create(null, new EventArgs());
            }

            private void BkgWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                RunPowershellScripts();
                e.Cancel = true;
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
            Task.Delay(1000);
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
            commands += GetCommandForSelectedBoard();
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
            //p.StandardInput.WriteLine("OEM");
            //p.StandardInput.WriteLine("FAM");
            //p.StandardInput.WriteLine("1234");
            //p.StandardInput.WriteLine("RPi2");
            //p.StandardInput.WriteLine("RPi2");
            p.StandardInput.WriteLine("Exit");
            p.WaitForExit();
            File.Delete(newFilePath);
        }
        public string GetCommandForSelectedBoard()
        {

            var command = "";
            var extractedPath = "";
            var locationArray = Location.Split('\\');
            var folderName = locationArray.LastOrDefault();
            var needsExtraction = SelectedManufacturer.PSImportCmd.Contains("<ExtractedPath>");
            if (needsExtraction)
            {
                extractedPath = @"c:\\temp\\" + Guid.NewGuid().ToString();
                System.IO.Compression.ZipFile.ExtractToDirectory(Location, extractedPath);
            }
            var bspName = SelectedManufacturer.BSPName.Replace("<FolderName>", folderName);
            if (SelectedManufacturer.MoveUp1Directory)
                Location = Path.GetDirectoryName(Location);
            command = SelectedManufacturer.PSImportCmd.Replace("<BSPName>", bspName).Replace("<Path>", Location);
            command = command.Replace("<ExtractedPath>", extractedPath);
            var commands = "\n" + "$newCmd = '" + command + "'" + "\n";
            commands += "invoke-expression  $newCmd";
            commands += "\n" + "$bldCmd = '" + SelectedManufacturer.BuildCmd.Replace("<BSPName>", bspName) + "'" + "\n";
            commands += "invoke-expression  $bldCmd";
            commands += "\n" + "$bldPkgAllCmd = 'New-IoTCabPackage All'" + "\n";
            commands += "invoke-expression  $bldPkgAllCmd";
            workspace.BSPName = SelectedManufacturer.BSPName;
            //commands += "\n" + "$addCmd = 'Add-IoTProduct ProductA " + bspName + "'" + "\n";
            //commands += "invoke-expression  $addCmd";
            return commands;
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
        System.Windows.Input.ICommand bsppathSelectionCommand;
        public ICommand BSPPathSelectionCommand
        {
            get { return bsppathSelectionCommand == null ? new DelegateCommand<object>(this.BSPPathSelectionCommandExec, x => true) : bsppathSelectionCommand; }
            set { SetProperty(ref pathSelectionCommand, value); }
        }
        private void BSPPathSelectionCommandExec(object parm)
        {
            if (SelectedManufacturer.PickFolder)
            {
                SelectFolder();
            }
            else
            {
                SelectFile();
            }
        }
        private void SelectFile()
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Filter = "RPi, Qualcom|*.zip;";
            if (Location != "Path not set.")
            {
                fileDialog.FileName = Location;
            }
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var file = fileDialog.FileName;
                    Location = file;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
        private void SelectFolder()
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

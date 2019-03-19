using AMaungUs.FFUMaker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class ImportBSPViewModel : BaseViewModel
    {

        PrerequisiteViewModel prerequisites;
        private List<BSPManufacturerInfo> bspManufacturerDefinitions;
        public List<BSPManufacturerInfo> BSPManufacturerDefinitions
        {
            get { return bspManufacturerDefinitions == null? new List<BSPManufacturerInfo>(): bspManufacturerDefinitions; }
            set { SetProperty(ref bspManufacturerDefinitions, value); }
        }
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
        BSPManufacturerInfo manufacturer;
        public BSPManufacturerInfo SelectedManufacturer
        {
            get { return manufacturer==null? new BSPManufacturerInfo(): manufacturer; }
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
            LoadJson();
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
            if (string.IsNullOrEmpty(BoardName) || string.IsNullOrEmpty(SelectedManufacturer.Manufacturer) || string.IsNullOrEmpty(Architecture) || string.IsNullOrEmpty(Location))
                return false;
            else
            {
                BoardSupportPackage = new BSP() { Architecture = Architecture, Manufacturer = SelectedManufacturer.Manufacturer, BoardName = BoardName, Location = Location };
                return true;
            }
        }
        private void RunPowershellScripts()
        {
            string file = prerequisites.AdkAddOnKitPath + "\\Tools\\LaunchShell.ps1";
            FileInfo fInfo = new FileInfo(file);
            var f = fInfo.DirectoryName;
            string newFilePath = f + @"\LaunchshellMod.ps1";
            string commands = string.Empty;
            using (StreamReader sr = new StreamReader(file))
            {
                // and plunk the contents in the textbox
                commands = sr.ReadToEnd();
            }
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
            p.WaitForExit(15000);
            p.StandardInput.WriteLine("exit");
            File.Delete(newFilePath);
        }

        public string GetCommandForSelectedBoard()
        {

            var command = "";
            var locationArray = Location.Split('\\');
            var folderName = locationArray.LastOrDefault();
            var needsExtraction = SelectedManufacturer.PSImportCmd.Contains("<ExtractedPath>");
            var bspName = SelectedManufacturer.BSPName.Replace("<FolderName>", folderName);
            command = SelectedManufacturer.PSImportCmd.Replace("<BSPName>", bspName).Replace("<Path>", Location);
            if (needsExtraction)
            {
                //command = command.Replace("<ExtractPath>",)
            }
            var commands = "\n" + "$newCmd = '" + command + "'" + "\n";
            commands += "invoke-expression  $newCmd";
            commands += "\n" + "$bldCmd = '" + SelectedManufacturer.BuildCmd.Replace("<BSPName>",bspName) + "'" + "\n";
            commands += "invoke-expression  $bldCmd";
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
            if(SelectedManufacturer.PickFolder)
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
            fileDialog.Filter = "RPi BSP|*.zip;";
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


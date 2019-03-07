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
            string cmdArg = prerequisites.AdkAddOnKitPath+ "\\IoTCorePShell.cmd";
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.ApartmentState = System.Threading.ApartmentState.STA;
            runspace.ThreadOptions = PSThreadOptions.UseCurrentThread;


            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();

            pipeline.Commands.AddScript(cmdArg);
            pipeline.Commands[0].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
            Collection<PSObject> results = pipeline.Invoke();
            var error = pipeline.Error.ReadToEnd();
            runspace.Close();

            if (error.Count >= 1)
            {
                string errors = "";
                foreach (var Error in error)
                {
                    errors = errors + " " + Error.ToString();
                }
            }
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

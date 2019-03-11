using AMaungUs.FFUMaker.Models;
using AMaungUs.FFUMaker.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class WorkspaceViewModel : BaseViewModel
    {
        ObservableCollection<Workspace> workspaces;
        public ObservableCollection<Workspace> Workspaces
        {
            get
            {
                return workspaces == null ? new ObservableCollection<Workspace>() : workspaces;
            }
            set {
                SetProperty(ref workspaces, value);
            }
        }
        Workspace selectedWorkspace;
        public Workspace SelectedWorkspace
        {
            get
            {
                return selectedWorkspace == null ? new Workspace() : selectedWorkspace;
            }
            set
            {
                SetProperty(ref selectedWorkspace, value);
            }
        }

        public WorkspaceViewModel()
        {
            
            var savedjson = Properties.Settings.Default.Workspaces;
            var _workspaces = JsonConvert.DeserializeObject<List<Workspace>>(savedjson);
            Workspaces = new ObservableCollection<Workspace>();
            Workspaces.Add(new Workspace { Architecture = "ARM", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x86", Name = "FFU Maker", OEMName = "CNCY", Path = "C:\\IoT\\Workspace\\Cncy\\Pi" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Snapdragon Test", OEMName = "CNCY", Path = "C:\\IoT\\WorkSpaces\\410C" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT1" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT2" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT3" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT4" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT5" });
            CreateWorkspaceCommand = new DelegateCommand<object>(this.CreateWorkspaceCmdExec, x => true);
            OnPropertyChanged("CreateWorkspaceCommand");
            DelWorkSpaceCommand = new DelegateCommand<object>(this.DelWorkspaceCommandExec, x => true);
            OnPropertyChanged("DelWorkSpaceCommand");
            SelectCommand = new DelegateCommand<object>(this.SelectCommandExec, x => true);
            OnPropertyChanged("SelectCommand");
            NextCommand = new DelegateCommand<object>(this.NextCommandExec, x => true);
            OnPropertyChanged("NextCommand");
            CleanUpWorkspace();
        }
        public void CleanUpWorkspace()
        {
            for(int i = 0; i< Workspaces.Count; i++)
            {
                var workspace = Workspaces[i];
                var shouldDelete = DeleteNonExistWorkspace(workspace);
                if (shouldDelete)
                {
                    Workspaces.RemoveAt(i);
                    SaveWorkspaces();
                    if(i>0)
                        i--;
                }
            }
        }
        System.Windows.Input.ICommand createWorkspaceCommand;
        public ICommand CreateWorkspaceCommand
        {
            get; set;
        }
        private void CreateWorkspaceCmdExec(object parm)
        {
            CreateWorkspaceModal createModal = new CreateWorkspaceModal();
            Nullable<bool> dialogresult = createModal.ShowDialog();
            var x = createModal.DataContext;
        }
        System.Windows.Input.ICommand delWorkSpaceCommand;
        public ICommand DelWorkSpaceCommand
        {
            get; set;
        }
        private void DelWorkspaceCommandExec(object parm)
        {
            if(parm is Workspace)
            {
                var deleteObject = (Workspace)parm;
                DeleteWorkspace(deleteObject);
            }
        }
        public bool DeleteNonExistWorkspace(Workspace workspace)
        {
            if (!Directory.Exists(workspace.Path))
                return true;
            else
                return false;
        }
        public void DeleteWorkspace(Workspace deleteObject)
        {
            try
            {
                Workspaces.Remove(deleteObject);
                DeleteFolder(deleteObject.Path);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                SaveWorkspaces();
            }
        }
        public void SaveWorkspaces()
        {
            var savejson = JsonConvert.SerializeObject(workspaces);
            Properties.Settings.Default.Workspaces = savejson;
            Properties.Settings.Default.Save();
        }
        private void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
        System.Windows.Input.ICommand selectCommand;
        public ICommand SelectCommand
        {
            get; set;
        }
        private void SelectCommandExec(object parm)
        {
            SelectedWorkspace = (Workspace)parm;
        }
        System.Windows.Input.ICommand nextCommand;
        public ICommand NextCommand
        {
            get; set;
        }
        private void NextCommandExec(object parm)
        {
        }
    }
}

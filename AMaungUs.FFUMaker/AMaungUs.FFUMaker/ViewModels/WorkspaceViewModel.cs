using AMaungUs.FFUMaker.Models;
using AMaungUs.FFUMaker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class WorkspaceViewModel : BaseViewModel
    {
        ObservableCollection<Workspace> workspace;
        public ObservableCollection<Workspace> Workspaces
        {
            get
            {
                return workspace == null ? new ObservableCollection<Workspace>() : workspace;
            }
            set {
                SetProperty(ref workspace, value);
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
            Workspaces = new ObservableCollection<Workspace>();
            Workspaces.Add(new Workspace { Architecture = "ARM", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x86", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            CreateWorkspaceCommand = new DelegateCommand<object>(this.CreateWorkspaceCmdExec, x => true);
            OnPropertyChanged("CreateWorkspaceCommand");
            DelWorkSpaceCommand = new DelegateCommand<object>(this.DelWorkspaceCommandExec, x => true);
            OnPropertyChanged("DelWorkSpaceCommand");
            SelectCommand = new DelegateCommand<object>(this.SelectCommandExec, x => true);
            OnPropertyChanged("SelectCommand");
            NextCommand = new DelegateCommand<object>(this.NextCommandExec, x => true);
            OnPropertyChanged("NextCommand");
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

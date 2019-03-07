using AMaungUs.FFUMaker.Models;
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
        public WorkspaceViewModel()
        {
            Workspaces = new ObservableCollection<Workspace>();
            Workspaces.Add(new Workspace { Architecture = "ARM", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x86", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            Workspaces.Add(new Workspace { Architecture = "x64", Name = "Test", OEMName = "CNCY", Path = "C:\\IoT" });
            AddWorkSpaceCommand = new DelegateCommand<object>(this.AddWorkspaceCommandExec, x => true);
            OnPropertyChanged("AddWorkSpaceCommand");
            DelWorkSpaceCommand = new DelegateCommand<object>(this.DelWorkspaceCommandExec, x => true);
            OnPropertyChanged("DelWorkSpaceCommand");
        }
        System.Windows.Input.ICommand addWorkSpaceCommand;
        public ICommand AddWorkSpaceCommand
        {
            get; set;
        }
        private void AddWorkspaceCommandExec(object parm)
        {
        }
        System.Windows.Input.ICommand delWorkSpaceCommand;
        public ICommand DelWorkSpaceCommand
        {
            get; set;
        }
        private void DelWorkspaceCommandExec(object parm)
        {
        }
    }
}

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
        public ObservableCollection<Workspace> WorkSpaces
        {
            get
            {
                return workspace == null ? new Workspace() : workspace;
            }
            set {
                SetProperty(ref workspace, value);
            }
        }
        public WorkspaceViewModel()
        {
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

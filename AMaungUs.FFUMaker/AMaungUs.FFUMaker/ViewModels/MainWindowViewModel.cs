using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class MainWindowViewModel: BaseViewModel
    {
        PrerequisiteViewModel prereqvm;
        public PrerequisiteViewModel PreReqVM
        {
            get
            {
                return prereqvm == null ? new PrerequisiteViewModel() : prereqvm;
            }
            set
            {
                SetProperty(ref prereqvm, value);
            }
        }
        WorkspaceViewModel workspaceVM;
        public WorkspaceViewModel WorkspaceVM
        {
            get
            {
                return workspaceVM == null ? new WorkspaceViewModel() : workspaceVM;
            }
            set
            {
                SetProperty(ref workspaceVM, value);
            }
        }
        public MainWindowViewModel()
        {
            PreReqVM = new PrerequisiteViewModel();
            WorkspaceVM = new WorkspaceViewModel();
        }
    }
}

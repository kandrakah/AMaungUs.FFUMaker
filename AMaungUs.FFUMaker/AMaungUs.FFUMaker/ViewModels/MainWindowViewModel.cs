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
        public MainWindowViewModel()
        {
            PreReqVM = new PrerequisiteViewModel();
        }
    }
}

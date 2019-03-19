using AMaungUs.FFUMaker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        ObservableCollection<Workspace> workspaces;
        Workspace _workspace;
        public string Title
        {
            get
            {
                return SelectedWorkspace.Name + " - Product(s)";
            }
        }
        public Workspace SelectedWorkspace
        {
            get
            {
                return _workspace == null ? new Workspace() : _workspace;
            }
            set
            {
                SetProperty(ref _workspace, value);
                OnPropertyChanged("Title");
            }
        }
        public ProductViewModel()
        {
            var savedjson = Properties.Settings.Default.Workspaces;
            var _workspaces = JsonConvert.DeserializeObject<ObservableCollection<Workspace>>(savedjson);
            workspaces = _workspaces;
        }
    }
}

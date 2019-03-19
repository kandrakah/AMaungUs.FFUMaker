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
    public class BSPViewModel : BaseViewModel
    {
        ObservableCollection<BSP> bspcollection;
        public ObservableCollection<BSP> BSPs
        {
            get
            {
                return bspcollection == null ? new ObservableCollection<BSP>() : bspcollection;
            }
            set {
                SetProperty(ref bspcollection, value);
            }
        }

        public BSPViewModel()
        {
            
            var savedjson = Properties.Settings.Default.BSPs;
            var _bspCollection = JsonConvert.DeserializeObject<ObservableCollection<BSP>>(savedjson);
            BSPs = _bspCollection;
            ImportBSPCommand = new DelegateCommand<object>(this.ImportBSPCommandExec, x => true);
            OnPropertyChanged("ImportBSPCommand");
        }
        System.Windows.Input.ICommand importBSPCommand;
        public ICommand ImportBSPCommand
        {
            get; set;
        }
        private void ImportBSPCommandExec(object parm)
        {
            ImportBSPModal importModal = new ImportBSPModal();
            Nullable<bool> dialogresult = importModal.ShowDialog();
            if (dialogresult.Value == true)
            {
                var bsp = ((ImportBSPViewModel)importModal.DataContext).BoardSupportPackage;
                BSPs.Add(bsp);
                SaveBoardSupportPackages();
            }
        }
        public void SaveBoardSupportPackages()
        {
            var savejson = JsonConvert.SerializeObject(bspcollection);
            Properties.Settings.Default.BSPs = savejson;
            Properties.Settings.Default.Save();
        }
       
    }
}

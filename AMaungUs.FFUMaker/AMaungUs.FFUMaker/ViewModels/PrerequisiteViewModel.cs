using AMaungUs.FFUMaker.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class PrerequisiteViewModel : BaseViewModel
    {
        private DispatcherTimer timer;
        PrereqDefinition prereqDefinition;
        public PrereqDefinition PrereqDefinition
        {
            get { return prereqDefinition==null?new PrereqDefinition(): prereqDefinition; }
            set { SetProperty(ref prereqDefinition, value); }
        }
        bool hasallprerequisites = false;
        public bool HasAllPreRequisites
        {
            get { return hasallprerequisites; }
            set { SetProperty(ref hasallprerequisites, value); }
        }
        bool hasWindowsADK = false;
        public bool HasWindowsADK
        {
            get { return hasWindowsADK; }
            set { SetProperty(ref hasWindowsADK, value); }
        }
        bool hasWindowsPE = false;
        public bool HasWindowsPE
        {
            get { return hasWindowsPE; }
            set { SetProperty(ref hasWindowsPE, value); }
        }
        bool hasIoTADKAddOnKit = false;
        public bool HasIoTADKAddOnKit
        {
            get { return hasIoTADKAddOnKit; }
            set { SetProperty(ref hasIoTADKAddOnKit, value); }
        }
        bool hasWindowsDriverKit = false;
        public bool HasWindowsDriverKit
        {
            get { return hasWindowsDriverKit; }
            set { SetProperty(ref hasWindowsDriverKit, value); }
        }
        bool hasWindowsIoTCorePackages = false;
        public bool HasWindowsIoTCorePackages
        {
            get { return hasWindowsIoTCorePackages; }
            set { SetProperty(ref hasWindowsIoTCorePackages, value); }
        }
        bool hasArmPackages = false;
        public bool HasArmPackages
        {
            get { return hasArmPackages; }
            set { SetProperty(ref hasArmPackages, value); }
        }
        bool hasArm64Packages = false;
        public bool HasArm64Packages
        {
            get { return hasArm64Packages; }
            set { SetProperty(ref hasArm64Packages, value); }
        }
        bool hasX86Packages = false;
        public bool HasX86Packages
        {
            get { return hasX86Packages; }
            set { SetProperty(ref hasX86Packages, value); }
        }
        bool hasX64Packages = false;
        public bool HasX64Packages
        {
            get { return hasX64Packages; }
            set { SetProperty(ref hasX64Packages, value); }
        }
        string adkAddOnKitPath;
        public string AdkAddOnKitPath
        {
            get { return adkAddOnKitPath; }
            set { SetProperty(ref adkAddOnKitPath, value); }
        }
        public PrerequisiteViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
            LoadJson();
            GetAdkAddOnKitPath();
            ValidatePreRequisites();
            DownloadCommand = new DelegateCommand<object>(this.DownloadCommandExecution, x => true);
            OnPropertyChanged("DownloadCommand");
            SetADKAddOnKitLocCmd = new DelegateCommand<object>(this.SetADKAddOnKitLocExec, x => true);
            OnPropertyChanged("SetADKAddOnKitLocCmd");
            ValidateCommand = new DelegateCommand<object>(this.ValidateCommandExecution, x => true);
            OnPropertyChanged("ValidateCommand");
        }
        private void Timer_Tick(object sender, object e)
        {
            if (!HasAllPreRequisites)
                ValidatePreRequisites();
            else
                timer.Stop();
        }
        private void LoadJson()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader r = new StreamReader(path + "/Assets/PrerequisiteLinks.json"))
            {
                string json = r.ReadToEnd();
                prereqDefinition = JsonConvert.DeserializeObject<PrereqDefinition>(json);
            }
        }
        private void ValidatePreRequisites()
        {
            ValidateWindowsADK();
            ValidateWindowsPE();
            ValidateIoTADKAddonKit();
            ValidateWindowsDriverKit();
            ValidateWindowsIoTCorePackages();
            if(HasWindowsADK && HasWindowsPE && HasWindowsIoTCorePackages && HasIoTADKAddOnKit)
            {
                HasAllPreRequisites = true;
            }
        }
        private void ValidateWindowsADK()
        {
            HasWindowsADK = RegKeyExists(prereqDefinition.WindowsADK.RegistryLocation[0]);
        }
        private void ValidateWindowsPE()
        {
            bool hasKey = false;
            foreach (var key in prereqDefinition.WindowsPE.RegistryLocation)
            {
                hasKey = RegKeyExists(key);
                if (hasKey == true)
                    break;
            }
            HasWindowsPE = hasKey;
        }
        private void ValidateIoTADKAddonKit()
        {
            HasIoTADKAddOnKit = File.Exists(AdkAddOnKitPath + "\\" + prereqDefinition.IoTADKAddonKit.VerificationLocation);
        }
        private void ValidateWindowsDriverKit()
        {
        }
        private void ValidateWindowsIoTCorePackages()
        {
            foreach (var key in prereqDefinition.WindowsIoTCorePackages.RegistryLocation)
            {
                var result = RegKeyExists(key);
                if (result == true)
                {
                    if (key.ToUpper().Contains("CORE_ARM64"))
                        HasArm64Packages = result;
                    else if (key.ToUpper().Contains("CORE_X86"))
                        HasX86Packages = result;
                    else if (key.ToUpper().Contains("CORE_X64"))
                        HasX64Packages = result;
                    else
                        HasArmPackages = result;
                }

            }
            HasWindowsIoTCorePackages = (HasArmPackages || HasArm64Packages || HasX86Packages || HasX64Packages);
        }
        private bool RegKeyExists(string path)
        {
            path = path.Replace("HKEY_CLASSES_ROOT", "");
            //opening the subkey  
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(path);
            bool keyFound = false;
            //if it does exist, retrieve the stored values  
            if (key != null)
            {
                keyFound = true;
            }
            else
            {
                keyFound = false;
            }
            key.Close();
            return keyFound;

        }
        private void GetAdkAddOnKitPath()
        {
            var value = Properties.Settings.Default.IoTADKAddonKitLocation;
            if (string.IsNullOrEmpty(value))
                AdkAddOnKitPath = "Path not set.";
            else
                AdkAddOnKitPath = value;
        }
        System.Windows.Input.ICommand downloadCommand;
        public ICommand DownloadCommand
        {
            get; set;
        }
        private void DownloadCommandExecution(object parm)
        {
            if (parm is string)
            {
                System.Diagnostics.Process.Start((string)parm);
            }
        }
        System.Windows.Input.ICommand setADKAddOnKitLocCmd;
        public ICommand SetADKAddOnKitLocCmd
        {
            get; set;
        }
        private void SetADKAddOnKitLocExec(object parm)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if(AdkAddOnKitPath != "Path not set.")
            {
                folderDialog.SelectedPath = AdkAddOnKitPath;
            }
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = folderDialog.SelectedPath;
                    AdkAddOnKitPath = folder;
                    ValidateIoTADKAddonKit();
                    if (!HasIoTADKAddOnKit)
                    {
                        MessageBox.Show("IoT-ADK-AddOnKit not found at the selected path.\n\n" + folder);
                        AdkAddOnKitPath = "Path not set.";
                    }
                    else
                    {
                        Properties.Settings.Default.IoTADKAddonKitLocation = folder;
                        Properties.Settings.Default.Save();
                    }
                    ValidatePreRequisites();
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
        System.Windows.Input.ICommand validateCommand;
        public ICommand ValidateCommand
        {
            get; set;
        }
        private void ValidateCommandExecution(object parm)
        {
            ValidatePreRequisites();
        }
    }
}

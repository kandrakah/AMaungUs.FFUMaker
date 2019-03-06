using AMaungUs.FFUMaker.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.ViewModels
{
    public class PrerequisiteViewModel : BaseViewModel
    {
        PrereqDefinition prereqDefinition;
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
        public PrerequisiteViewModel()
        {
            LoadJson();
            ValidatePreRequisites();
        }
        public void LoadJson()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader r = new StreamReader(path + "/Assets/PrerequisiteLinks.json"))
            {
                string json = r.ReadToEnd();
                prereqDefinition = JsonConvert.DeserializeObject<PrereqDefinition>(json);
            }
        }
        public void ValidatePreRequisites()
        {
            ValidateWindowsADK();
            ValidateWindowsPE();
            ValidateIoTADKAddonKit();
            ValidateWindowsDriverKit();
            ValidateWindowsIoTCorePackages();
            if(HasWindowsADK && hasWindowsPE && HasWindowsIoTCorePackages)
            {
                HasAllPreRequisites = true;
            }
        }
        public void ValidateWindowsADK()
        {
            HasWindowsADK = RegKeyExists(prereqDefinition.WindowsADK.RegistryLocation[0]);
        }
        public void ValidateWindowsPE()
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
        public void ValidateIoTADKAddonKit()
        {
        }
        public void ValidateWindowsDriverKit()
        {
        }
        public void ValidateWindowsIoTCorePackages()
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
    }
}

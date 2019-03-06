using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.Models
{
    public class Prerequisite
    {
        public string DisplayName { get; set; }
        public string DownloadLink { get; set; }
        public string Optional { get; set; }
        public string VerificationLocation { get; set; }
        public List<string> RegistryLocation { get; set; }
        public string VerificationCommand { get; set; }
    }
    public class PrereqDefinition
    {
        public Prerequisite WindowsADK { get; set; }
        public Prerequisite WindowsPE { get; set; }
        public Prerequisite IoTADKAddonKit { get; set; }
        public Prerequisite WindowsDriverKit { get; set; }
        public Prerequisite WindowsIoTCorePackages { get; set; }
    }
}

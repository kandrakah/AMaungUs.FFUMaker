using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.Models
{
    public class BSPManufacturerInfo
    {
        public string Manufacturer { get; set; }
        public string Board { get; set; }
        public string PSImportCmd { get; set; }
        public string BSPName { get; set; }
        public string BuildCmd { get; set; }
        public bool NeedsExtraction { get; set; }
        public string Info { get; set; }
        public bool PickFolder { get; set; }
        public bool MoveUp1Directory { get; set; }
    }
}

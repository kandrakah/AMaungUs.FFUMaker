using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMaungUs.FFUMaker.Models
{
    public class Product
    {
        public string Path { get; set; }
        public string ProductName
        {
            get
            {
                if (string.IsNullOrEmpty(Path))
                    return "";
                var locationArray = Path.Split('\\');
                var folderName = locationArray.LastOrDefault();
                return folderName;
            }
        }
    }
}

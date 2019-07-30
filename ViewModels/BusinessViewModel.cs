using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class BusinessViewModel
    {
        public string Name { get; set; }
        public BusinessType Type { get; set; } = BusinessType.Winkel;
        public string Address { get; set; }
    }
}

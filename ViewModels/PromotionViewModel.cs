using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class PromotionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public string Description { get; set; }
    }
}

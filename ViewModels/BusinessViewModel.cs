using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Entities;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class BusinessViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BusinessType Type { get; set; } = BusinessType.Winkel;
        public string Address { get; set; }
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
        public List<PromotionViewModel> Promotions { get; set; } = new List<PromotionViewModel>();
        public string ImageUrl { get; set; }
        public Uri ImageUri { get; set; }
    }
}

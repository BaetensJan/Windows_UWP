using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<EventViewModel> Events { get; set; } = new ObservableCollection<EventViewModel>();
        public ObservableCollection<PromotionViewModel> Promotions { get; set; } = new ObservableCollection<PromotionViewModel>();
        public string ImageUrl { get; set; }
        public Uri ImageUri { get; set; }
    }
}

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
        public string Name { get; set; }
        public BusinessType Type { get; set; } = BusinessType.Winkel;
        public string Address { get; set; }
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>();
        public string ImageUrl { get; set; }
        public Uri ImageUri { get; set; }

        public void ParseBusiness(Business business)
        {
            Name = business.Name;
            Type = business.Type;
            Address = business.Address;
            ImageUrl = business.ImageUrl;
            ImageUri = new Uri(business.ImageUrl);
            foreach (var ev in business.Events)
            {
                Events.Add(new EventViewModel()
                {
                    Id = ev.Id,
                    Name = ev.Name,
                    Description = ev.Description,
                    Type = ev.Type
                });
            }
        }
    }
}

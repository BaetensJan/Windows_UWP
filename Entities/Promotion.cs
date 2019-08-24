using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Enums;

namespace Windows_UWP.Entities
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PromotionType PromotionType { get; set; }
        public string StartAndEndDate { get; set; }
        public string Description { get; set; }
        public DateTime Creation { get; set; }
    }
}

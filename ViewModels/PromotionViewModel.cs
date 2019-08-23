using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class PromotionViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _name;
        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private PromotionType _promotionType;
        public PromotionType PromotionType {
            get { return _promotionType; }
            set {
                _promotionType = value;
                OnPropertyChanged("PromotionType");
            }
        }

        private string _startAndEndDate;
        public string StartAndEndDate {
            get { return _startAndEndDate; }
            set {
                _startAndEndDate = value;
                OnPropertyChanged("StartAndEndDate");
            }
        }
        public string _description;

        public string Description {
            get { return _description; }
            set {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

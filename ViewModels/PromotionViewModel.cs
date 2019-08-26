using System;
using System.ComponentModel;
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
                if (value == "" && value == null)
                {
                    throw new Exception("Naam van de promotie moet ingevuld zijn.");
                }
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private PromotionType _promotionType = PromotionType.Andere;
        public PromotionType PromotionType {
            get { return _promotionType; }
            set {
                _promotionType = value;
                OnPropertyChanged("PromotionType");
            }
        }

        private DateTimeOffset _startDateOffset;
        public DateTimeOffset StartDateOffset {
            get { return _startDateOffset; }
            set {
                _startDateOffset = value;
                OnPropertyChanged("StartDate");
            }
        }

        private DateTimeOffset _endDateOffset;
        public DateTimeOffset EndDateOffset {
            get { return _endDateOffset; }
            set {
                _endDateOffset = value;
                OnPropertyChanged("EndDate");
            }
        }
        public string EndDate {
            set { EndDateOffset = DateTimeOffset.Parse(value); }
            get { return _endDateOffset.ToString(); }

        }

        public string StartDate {
            set { StartDateOffset = DateTimeOffset.Parse(value); }
            get { return _startDateOffset.ToString(); }

        }

        public string _description;

        public string Description {
            get { return _description; }
            set {
                if (value == "" && value == null)
                {
                    throw new Exception("Description van de promotie moet ingevuld zijn.");
                }
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        public DateTime Creation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public PromotionViewModel()
        {
            EndDateOffset = DateTimeOffset.Now;
            StartDateOffset = DateTimeOffset.Now;
        }
    }
}

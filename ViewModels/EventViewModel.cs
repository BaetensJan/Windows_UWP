using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_UWP.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string _name;
        public string Name {
            get { return _name; }
            set {
                if (value == "" && value == null)
                {
                    throw new Exception("Naam van het event moet ingevuld zijn.");
                }
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string _type;
        public string Type {
            get { return _type; }
            set {
                if (value == "" && value == null)
                {
                    throw new Exception("Kies een type voor het event.");
                }
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        public string _description;

        public string Description
        {
            
    get { return _description; }
            set {
                if (value == "" && value == null)
                {
                    throw new Exception("Description van het event moet ingevuld zijn.");
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
    }
}

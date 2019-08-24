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
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string _type;
        public string Type {
            get { return _type; }
            set {
                _type = value;
                OnPropertyChanged("Type");
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
        public DateTime Creation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

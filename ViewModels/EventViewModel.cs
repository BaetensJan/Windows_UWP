using System;
using System.ComponentModel;

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
                    throw new Exception("Name of event needs to be filled in.");
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
                    throw new Exception("Choose a type for the event.");
                }
                _type = value;
                OnPropertyChanged("Type");
            }
        }
        public string _description;

        public string Description {

            get { return _description; }
            set {
                if (value == "" && value == null)
                {
                    throw new Exception("Enter a description for the event.");
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

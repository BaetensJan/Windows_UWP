using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Windows_UWP
{
    public class UserSettings : INotifyPropertyChanged
    {

        private string _JWTToken;
        public string JWTToken {
            get { return _JWTToken; }
            set {
                _JWTToken = value;
                IsLoggedIn = (value != null || value.Length != 0);
                OnPropertyChanged();
            }
        }

        private bool _isLoggedIn = false;
        public bool IsLoggedIn {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

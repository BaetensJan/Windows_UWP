using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Data;

namespace Windows_UWP.ViewModels
{
    public class EventsViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const string apiUrl = "http://localhost:5000/Business";
        private List<BusinessViewModel> _businessViewModel;
        public List<BusinessViewModel> BusinessViewModel
        {
            get { return _businessViewModel; }
            set
            {
                _businessViewModel = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("BusinessViewModel");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task LoadDataAsync()
        {
            try
            {
                BusinessViewModel = await ApiClient.Instance.GetAbboneeBusinessByEmailFromJWTAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

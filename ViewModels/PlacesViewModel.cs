using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows_UWP.Entities;

namespace Windows_UWP.ViewModels
{
    public class PlacesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Business> _Items;
        public ObservableCollection<Business> Items {
            get { return _Items; }
            set {
                _Items = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Items");
            }
        }

        private const string apiUrl = "http://localhost:5000/business/index";

        public event PropertyChangedEventHandler PropertyChanged;

        public PlacesViewModel()
        {
            Items = new ObservableCollection<Business>();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task LoadDataAsync()
        {
            //Todo: move to data layer and call in viewmodel

            try
            {
                HttpClient client = new HttpClient();
                var json = await client.GetStringAsync(apiUrl);
                Items = JsonConvert.DeserializeObject<ObservableCollection<Business>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

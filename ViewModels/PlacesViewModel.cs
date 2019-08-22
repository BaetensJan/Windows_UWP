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
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class PlacesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Business> _items;
        public ObservableCollection<Business> Items {
            get { return _items; }
            set {
                _items = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Items");
            }
        }

        public ObservableCollection<Business> _filteredItems;
        public ObservableCollection<Business> FilteredItems {
            get { return _filteredItems; }
            set { _filteredItems = value; OnPropertyChanged("FilteredItems"); }
        }

        private const string apiUrl = "http://localhost:5000/business/index";

        public event PropertyChangedEventHandler PropertyChanged;

        public PlacesViewModel()
        {
            Items = new ObservableCollection<Business>();
            FilteredItems = new ObservableCollection<Business>();
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

        public void Filter(string name, BusinessType? businessType)
        {
            var filtered = Items.Where(b => b.Name.Contains(name)).ToList();
            if (businessType != null)
            {
                filtered = filtered.Where(b => b.Type.Equals(businessType)).ToList();
            }

            // This will limit the amount of view refreshes 
            if (FilteredItems.Count == filtered.Count())
                return;

            FilteredItems = new ObservableCollection<Business>(filtered);

        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows_UWP.Data;
using Windows_UWP.Enums;

namespace Windows_UWP.ViewModels
{
    public class PlacesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BusinessViewModel> _items;
        public ObservableCollection<BusinessViewModel> Items {
            get { return _items; }
            set {
                _items = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Items");
            }
        }

        public ObservableCollection<BusinessViewModel> _filteredItems;
        public ObservableCollection<BusinessViewModel> FilteredItems {
            get { return _filteredItems; }
            set { _filteredItems = value; OnPropertyChanged("FilteredItems"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PlacesViewModel()
        {
            Items = new ObservableCollection<BusinessViewModel>();
            FilteredItems = new ObservableCollection<BusinessViewModel>();
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
                var list = await ApiClient.Instance.GetPlacesAsync();
                foreach (var i in list)
                {
                    Items.Add(i);
                }
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

            FilteredItems = new ObservableCollection<BusinessViewModel>(filtered);

        }
    }
}

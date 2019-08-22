using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.Entities;
using Windows_UWP.Enums;
using Windows_UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlacesView : Page
    {
        public PlacesViewModel PlacesViewModel { get; set; } = new PlacesViewModel();

        public PlacesView()
        {
            this.InitializeComponent();
            PlacesViewModel.PropertyChanged += ItemListener;

            var enumVals = Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>();
            IList<BusinessType?> list = new List<BusinessType?>();
            list.Add(null);
            foreach (var type in enumVals)
            {
                list.Add(type);
            }
            businessType.ItemsSource = list;
            businessType.SelectedIndex = 0;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await PlacesViewModel.LoadDataAsync();
            PlacesViewModel.Filter("", null);
        }

        private void ItemListener(object sender, PropertyChangedEventArgs e)
        {
            PlacesGridView.ItemsSource = PlacesViewModel.FilteredItems;
        }

        private void PlacesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView listView = (GridView)sender;
            var clickedMenuItem = (Business)e.ClickedItem;
            Frame.Navigate(typeof(PlaceView), clickedMenuItem);
        }

        private void FilterOnChanged(object sender, RoutedEventArgs e)
        {
            var category = (BusinessType?)businessType.SelectedItem;
            PlacesViewModel.Filter(businessName.Text, category);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.Enums;
using Windows_UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPlaceView : Page
    {
        public EditPlaceViewModel EditPlaceViewModel { get; set; } = new EditPlaceViewModel();
        public EventViewModel EventViewModel { get; set; } = new EventViewModel();
        public PromotionViewModel PromotionViewModel { get; set; } = new PromotionViewModel();

        private BasicGeoposition gentLocation = new BasicGeoposition { Latitude = 51.05, Longitude = 3.71666667 };

        public EditPlaceView()
        {
            this.InitializeComponent();
            BingMap.Loaded += Map_Loaded;

            EditPlaceViewModel.PropertyChanged += ItemListener;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var enumVals = Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>();
            businessType.ItemsSource = enumVals.ToList();

            var promotionEnumVals = Enum.GetValues(typeof(PromotionType)).Cast<PromotionType>();
            promotionType.ItemsSource = promotionEnumVals.ToList();

            await EditPlaceViewModel.LoadDataAsync();
        }

        private void ItemListener(object sender, PropertyChangedEventArgs e)
        {
            GeocodePoint(EditPlaceViewModel.BusinessViewModel.Address);
            EventsGridView.ItemsSource = new ObservableCollection<EventViewModel>(EditPlaceViewModel.BusinessViewModel.Events);
            PromotionsGridView.ItemsSource = new ObservableCollection<PromotionViewModel>(EditPlaceViewModel.BusinessViewModel.Promotions);

        }

        private void BusinessType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (BusinessType type in (BusinessType[])Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>())
            {
                if (e.AddedItems[0].ToString().Equals(type.ToString()))
                {
                    EditPlaceViewModel.BusinessViewModel.Type = type;
                }
            }
        }
        private void PromotionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (PromotionType type in (PromotionType[])Enum.GetValues(typeof(PromotionType)).Cast<PromotionType>())
            {
                if (e.AddedItems[0].ToString().Equals(type.ToString()))
                {
                    PromotionViewModel.PromotionType = type;
                }
            }
        }

        private async void SubmitOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await EditPlaceViewModel.SaveTaskAsync();
                Notification.Show(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void EventsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView listView = (GridView)sender;
            var clickedMenuItem = (EventViewModel)e.ClickedItem;
            EventViewModel.Id = clickedMenuItem.Id;
            EventViewModel.Name = clickedMenuItem.Name;
            EventViewModel.Description = clickedMenuItem.Description;
            EventViewModel.Type = clickedMenuItem.Type;
        }

        private async void RemoveSelectedEvent(object sender, RoutedEventArgs e)
        {
            var removeEvent = (EventViewModel)EventsGridView.SelectedItem;
            await EditPlaceViewModel.RemoveEventFromBusiness(removeEvent);

            EditPlaceViewModel.BusinessViewModel.Events.Remove(removeEvent);

        }

        private async void RemoveSelectedPromotion(object sender, RoutedEventArgs e)
        {
            var removePromotion = (PromotionViewModel)PromotionsGridView.SelectedItem;
            await EditPlaceViewModel.RemovePromotionFromBusiness(removePromotion);

            EditPlaceViewModel.BusinessViewModel.Promotions.Remove(removePromotion);
        }

        private async void OnAddEventButton(object sender, RoutedEventArgs e)
        {
            try
            {
                EditPlaceViewModel.BusinessViewModel.Events.Add(EventViewModel);

                await EditPlaceViewModel.AddEventToBusiness();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void OnAddPromotionButton(object sender, RoutedEventArgs e)
        {
            try
            {

                EditPlaceViewModel.BusinessViewModel.Promotions.Add(PromotionViewModel);
                await EditPlaceViewModel.AddPromotionToBusiness();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PromotionsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView listView = (GridView)sender;
            var clickedMenuItem = (PromotionViewModel)e.ClickedItem;
            PromotionViewModel.Id = clickedMenuItem.Id;
            PromotionViewModel.Name = clickedMenuItem.Name;
            PromotionViewModel.Description = clickedMenuItem.Description;
            PromotionViewModel.PromotionType = clickedMenuItem.PromotionType;
            PromotionViewModel.StartAndEndDate = clickedMenuItem.StartAndEndDate;
        }

        #region Map Methods
        private async void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Geopoint gentPoint = new Geopoint(gentLocation);
            await BingMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(gentPoint, 5000));
        }

        private void MapOnDoubleClick(object sender, MapInputEventArgs args)
        {
            var currentCamera = BingMap.ActualCamera;
            _ = BingMap.TrySetSceneAsync(MapScene.CreateFromCamera(currentCamera));

            var geopoint = args.Location;
            AddMarker(geopoint);
            ReverseGeocodePoint(geopoint);
        }

        private void SearchOnClick(object sender, RoutedEventArgs e)
        {
            if (searchRequest.Text.Trim().Length != 0)
            {
                GeocodePoint(searchRequest.Text);
            }
        }

        private async void AddMarker(Geopoint geopoint)
        {
            var marker = new MapIcon
            {
                Location = geopoint,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                ZIndex = 0,
                Title = "Your Shop"
            };
            var locationMarkers = new List<MapElement>();

            locationMarkers.Add(marker);
            var markersLayer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = locationMarkers
            };
            BingMap.Layers.Clear();
            BingMap.Layers.Add(markersLayer);
            await BingMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(geopoint, 5000));
        }

        private async void GeocodePoint(string addressToGeocode)
        {
            try
            {
                Geopoint hintPoint = new Geopoint(gentLocation);
                MapLocationFinderResult result =
                    await MapLocationFinder.FindLocationsAsync(addressToGeocode, hintPoint, 1);

                // If the query returns results, display the name of the town
                // contained in the address of the first result.
                if (result.Status == MapLocationFinderStatus.Success && result.Locations.Count >= 0)
                {
                    AddMarker(result.Locations[0].Point);
                    ReverseGeocodePoint(result.Locations[0].Point);
                }
            }
            catch (Exception e)
            {

            }
        }

        private async void ReverseGeocodePoint(Geopoint geopoint)
        {
            try
            {

                // Reverse geocode the specified geographic location.
                MapLocationFinderResult result =
                      await MapLocationFinder.FindLocationsAtAsync(geopoint, MapLocationDesiredAccuracy.High);

                // If the query returns results, display the name of the town
                // contained in the address of the first result.
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    var locationStrings = result.Locations[0].Address;
                    businessAddress.Text = $"{locationStrings.Street} {locationStrings.StreetNumber} {locationStrings.Town} {locationStrings.Country}";
                    EditPlaceViewModel.BusinessViewModel.Address = businessAddress.Text;
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion


    }
}

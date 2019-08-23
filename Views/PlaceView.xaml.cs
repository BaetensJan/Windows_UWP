using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.Entities;
using Windows_UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaceView : Page
    {
        public BusinessViewModel BusinessViewModel { get; set; } = new BusinessViewModel();
        public UserBusinessViewModel UserBusinessViewModel { get; set; } = new UserBusinessViewModel();
        public Business business;
        private BasicGeoposition gentLocation = new BasicGeoposition { Latitude = 51.05, Longitude = 3.71666667 };

        public PlaceView()
        {
            this.InitializeComponent();
            BingMap.Loaded += Map_Loaded;
            PromotionsGridView.ItemsSource = BusinessViewModel.Promotions;
            EventsGridView.ItemsSource = BusinessViewModel.Events;
            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                business = (Business)e.Parameter;

                //TODO: CHANGE THIS
                business.ImageUrl = "https://fashiongrabber.blob.core.windows.net/shop-2659/IMG_2953.JPG";
                BusinessViewModel.ParseBusiness(business);
                UserBusinessViewModel.BusinessId = business.Id;
                await UserBusinessViewModel.CheckUserBusinessForSubscribtion();
                checkSubscribeButton();
                GeocodePoint(BusinessViewModel.Address);
            }
            catch (Exception ex)
            {

            }
        }

        private void GridViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void Subscribe(object sender, RoutedEventArgs e)
        {
            UserBusinessViewModel.BusinessId = business.Id;
            await UserBusinessViewModel.Subscribe();
            checkSubscribeButton();
        }

        private void checkSubscribeButton()
        {
            if (UserBusinessViewModel.substatus)
            {
                subscribeButton.Content = "Unsubscribe";
            }
            else
            {
                subscribeButton.Content = "Subscribe";
            }
        }

        #region Map Methods
        private async void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Geopoint gentPoint = new Geopoint(gentLocation);
            await BingMap.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(gentPoint, 5000));
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
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}

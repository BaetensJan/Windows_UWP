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
using Windows.Networking.BackgroundTransfer;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.Data;
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
        private BasicGeoposition gentLocation = new BasicGeoposition { Latitude = 51.05, Longitude = 3.71666667 };

        public PlaceView()
        {
            this.InitializeComponent();
            BingMap.Loaded += Map_Loaded;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                BusinessViewModel = (BusinessViewModel)e.Parameter;

                //TODO: CHANGE THIS
                BusinessViewModel.ImageUrl = "https://fashiongrabber.blob.core.windows.net/shop-2659/IMG_2953.JPG";
                UserBusinessViewModel.BusinessId = BusinessViewModel.Id;
                await UserBusinessViewModel.CheckUserBusinessForSubscription();
                checkSubscribeButton();
                GeocodePoint(BusinessViewModel.Address);
                PromotionsGridView.ItemsSource = BusinessViewModel.Promotions;
                EventsGridView.ItemsSource = BusinessViewModel.Events;
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
            UserBusinessViewModel.BusinessId = BusinessViewModel.Id;
            await UserBusinessViewModel.Subscribe();
            checkSubscribeButton();
        }

        private void checkSubscribeButton()
        {
            if (UserBusinessViewModel.SubStatus)
            {
                subscribeButton.Content = "Unsubscribe";
            }
            else
            {
                subscribeButton.Content = "Subscribe";
            }
        }

        private async void DownloadPDF(object sender, ItemClickEventArgs e)
        {
            var promotion = (PromotionViewModel)e.ClickedItem;
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = $"Promotion_{promotion.Id}.pdf";

            StorageFile file = await savePicker.PickSaveFileAsync();

            try
            {
                await ApiClient.Instance.GetPDF(promotion.Id, file);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

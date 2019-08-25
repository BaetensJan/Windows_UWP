using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;
using Windows_UWP.Data;
using Windows_UWP.Enums;
using Windows_UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_UWP.Views
{
    public sealed partial class RegisterView : Page
    {
        private BasicGeoposition gentLocation = new BasicGeoposition { Latitude = 51.05, Longitude = 3.71666667 };

        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();

        public RegisterView()
        {
            this.InitializeComponent();
            BingMap.Loaded += Map_Loaded;

            var enumVals = Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>();
            businessType.ItemsSource = enumVals.ToList();
            businessType.SelectedIndex = 0;
        }

        private void UserTypeOnClick(object sender, RoutedEventArgs e)
        {
            if (userType.IsChecked.GetValueOrDefault())
            {
                RegisterViewModel.UserType = UserType.Business;
                businessPanel.Visibility = Visibility.Visible;
            }
            else
            {
                RegisterViewModel.UserType = UserType.Customer;
                businessPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void BusinessType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (BusinessType type in (BusinessType[])Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>())
            {
                if (e.AddedItems[0].ToString().Equals(type.ToString()))
                {
                    RegisterViewModel.Business.Type = type;
                }
            }
        }

        private async void SubmitOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    await ApiClient.Instance.PostRegisterAsync(RegisterViewModel);
                    Frame.GoBack();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private bool ValidateInputs()
        {
            var valid = true;
            email.ClearValue(TextBox.BorderBrushProperty);
            password.ClearValue(TextBox.BorderBrushProperty);
            businessName.ClearValue(TextBox.BorderBrushProperty);
            businessAddress.ClearValue(TextBox.BorderBrushProperty);

            EmailValidationMessage.Visibility = Visibility.Collapsed;
            PasswordValidationMessage.Visibility = Visibility.Collapsed;
            NameValidationMessage.Visibility = Visibility.Collapsed;
            AddressValidationMessage.Visibility = Visibility.Collapsed;


            if (RegisterViewModel.Email == null || RegisterViewModel.Email.Trim().Length == 0)
            {
                email.BorderBrush = new SolidColorBrush(Colors.Red);
                valid = false;
                EmailValidationMessage.Visibility = Visibility.Visible;
            }
            if (RegisterViewModel.Password == null || RegisterViewModel.Password.Length == 0)
            {
                password.BorderBrush = new SolidColorBrush(Colors.Red);
                valid = false;
                PasswordValidationMessage.Visibility = Visibility.Visible;
            }
            if (RegisterViewModel.UserType.Equals(UserType.Business))
            {
                if (RegisterViewModel.Business.Name == null || RegisterViewModel.Business.Name.Trim().Length == 0)
                {
                    businessName.BorderBrush = new SolidColorBrush(Colors.Red);
                    valid = false;
                    NameValidationMessage.Visibility = Visibility.Visible;
                }
                if (RegisterViewModel.Business.Address == null || RegisterViewModel.Business.Address.Trim().Length == 0)
                {
                    businessAddress.BorderBrush = new SolidColorBrush(Colors.Red);
                    valid = false;
                    AddressValidationMessage.Visibility = Visibility.Visible;
                }
            }
            return valid;

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
                    RegisterViewModel.Business.Address = businessAddress.Text;
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}

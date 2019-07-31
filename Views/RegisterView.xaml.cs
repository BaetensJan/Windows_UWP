using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows_UWP.Enums;
using Windows_UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows_UWP.Views
{
    public sealed partial class RegisterView : Page
    {

        private const string apiUrl = "http://localhost:5000/account/register";
        private HttpClient client { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; } = new RegisterViewModel();

        public RegisterView()
        {
            this.InitializeComponent();
            client = new HttpClient();

            var enumVals = Enum.GetValues(typeof(BusinessType)).Cast<BusinessType>();
            businessType.ItemsSource = enumVals.ToList();
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
                var userJson = JsonConvert.SerializeObject(RegisterViewModel);
                HttpClient client = new HttpClient();
                var res = await client.PostAsync(apiUrl, new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));
                (App.Current as App).JWTToken = await res.Content.ReadAsStringAsync();
                Frame.GoBack();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

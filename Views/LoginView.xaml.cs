using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.ViewModels;

namespace Windows_UWP.Views
{

    public sealed partial class LoginView : Page
    {

        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();
        private const string apiUrl = "http://localhost:5000/account/login";

        public LoginView()
        {
            this.InitializeComponent();
        }
        private async void SubmitOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var userJson = JsonConvert.SerializeObject(LoginViewModel);
                HttpClient client = new HttpClient();
                var res = await client.PostAsync(apiUrl, new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));
                ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken = await res.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void CreateAccountOnClick(object sender, RoutedEventArgs e)
        {
            ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken = null;
            Frame.Navigate(typeof(RegisterView));
        }
    }
}

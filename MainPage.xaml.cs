using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows_UWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Windows_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            Application.Current.Resources["UserSettings"] = new UserSettings();
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            MainFrame.Navigate(typeof(LoginView));
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                e.Handled = true;
                MainFrame.GoBack();
            }
        }

        private void OnCloseButtonTapped(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private void OnBackButtonTapped(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void OnLoginButtonTapped(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(LoginView));
        }

        private void OnPlacesButtonTapped(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(PlacesView));
        }

        private void OnLogoutButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken = null;
            Frame.BackStack.Clear();
            MainFrame.Navigate(typeof(LoginView));
        }

        private void OnEditPlaceButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(EditPlaceView));
        }
        private void OnSeeAbboneePromotionsAndEvents(object sender, TappedRoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(EventsView));
        }
    }
}

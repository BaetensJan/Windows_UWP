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
        private string apiUrl = "http://localhost:5000/business/";
        public BusinessViewModel BusinessViewModel { get; set; } = new BusinessViewModel();

        public PlaceView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                var business = (Business)e.Parameter;
                business.ImageUrl = "https://fashiongrabber.blob.core.windows.net/shop-2659/IMG_2953.JPG";
                BusinessViewModel.ParseBusiness(business);
            }
            catch (Exception ex)
            {

            }
        }
    }
}

﻿using Newtonsoft.Json;
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
        public BusinessViewModel BusinessViewModel { get; set; } = new BusinessViewModel();
        public UserBusinessViewModel UserBusinessViewModel { get; set; } = new UserBusinessViewModel();
        public Business business;

        public PlaceView()
        {
            this.InitializeComponent();
            EventsGridView.ItemsSource = BusinessViewModel.Events;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                business = (Business)e.Parameter;
                business.ImageUrl = "https://fashiongrabber.blob.core.windows.net/shop-2659/IMG_2953.JPG";
                BusinessViewModel.ParseBusiness(business);
                UserBusinessViewModel.BusinessId = business.Id;
                await UserBusinessViewModel.CheckUserBusinessForSubscribtion();
                checkSubscribeButton();                
            }
            catch (Exception ex)
            {

            }
        }

        private void EventsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

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
    }
}

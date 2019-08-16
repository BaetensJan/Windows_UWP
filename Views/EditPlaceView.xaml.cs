﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        public EditPlaceView()
        {
            this.InitializeComponent();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void EventsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {


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
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PromotionsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}

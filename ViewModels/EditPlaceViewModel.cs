using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows_UWP.Data;
using Windows_UWP.Entities;

namespace Windows_UWP.ViewModels
{
    public class EditPlaceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private const string apiUrl = "http://localhost:5000/Business";
        private BusinessViewModel _businessViewModel;
        public BusinessViewModel BusinessViewModel {
            get { return _businessViewModel; }
            set {
                _businessViewModel = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("BusinessViewModel");
            }
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task LoadDataAsync()
        {
            try
            {
                BusinessViewModel = await ApiClient.Instance.GetBusinessFromJWTAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SaveTaskAsync()
        {
            try
            {
                await ApiClient.Instance.PostBusinessFromJWTAsync(BusinessViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //TODO: Remove? Add to save task?
        public async Task AddEventToBusiness()
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(BusinessViewModel);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/AddEvents", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task RemoveEventFromBusiness(EventViewModel removeEvent)
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(removeEvent);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/RemoveEvents", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }

        public async Task RemovePromotionFromBusiness(PromotionViewModel removePromotion)
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(removePromotion);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/RemovePromotion", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task AddPromotionToBusiness()
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;
                var json = JsonConvert.SerializeObject(BusinessViewModel);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);

                var res = await client.PostAsync($"{apiUrl}/AddPromotion", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task EditPromotion(PromotionViewModel promotionViewModel)
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(promotionViewModel);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);

                var res = await client.PostAsync($"{apiUrl}/EditPromotion", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }
        public async Task EditEvent(EventViewModel eventViewModel)
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(eventViewModel);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);

                var res = await client.PostAsync($"{apiUrl}/EditEvent", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                OnPropertyChanged("BusinessViewModel");
            }
            catch (Exception ex)
            {

            }
        }
    }
}

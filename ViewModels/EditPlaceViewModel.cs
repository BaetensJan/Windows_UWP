using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows_UWP.Utils;

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
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;

                var claims = JWTTokenConverter.ConvertToList(token);
                var businessId = claims.First(c => c.Type == "businessId").Value;
                HttpClient client = new HttpClient();
                var json = await client.GetStringAsync($"{apiUrl}/Index/{businessId}");
                BusinessViewModel = JsonConvert.DeserializeObject<BusinessViewModel>(json);
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
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(BusinessViewModel);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/Edit", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {

            }
        }

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
            }
            catch (Exception ex)
            {

            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows_UWP.Entities;
using Windows_UWP.Utils;
using Windows_UWP.ViewModels;

namespace Windows_UWP.Data
{
    public class ApiClient
    {
        private static ApiClient instance = null;
        private const string apiUrl = "http://localhost:5000";
        private HttpClient client = new HttpClient();
        private UserSettings UserSettings { get; set; }

        public static ApiClient Instance {
            get {
                if (instance == null)
                {
                    instance = new ApiClient();
                }
                return instance;
            }
        }

        private ApiClient()
        {
            UserSettings = (UserSettings)Application.Current.Resources["UserSettings"];
        }

        public async Task PostLoginAsync(LoginViewModel loginViewModel)
        {
            var userJson = JsonConvert.SerializeObject(loginViewModel);
            var res = await client.PostAsync($"{apiUrl}/Account/Login", new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode)
            {
                throw new ArgumentException("Error whilst logging in!");
            }
            UserSettings.JWTToken = await res.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", UserSettings.JWTToken);
        }

        public async Task PostRegisterAsync(RegisterViewModel registerViewModel)
        {
            var userJson = JsonConvert.SerializeObject(registerViewModel);
            var res = await client.PostAsync($"{apiUrl}/Account/Register", new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode)
            {
                throw new ArgumentException("Error whilst registering!");
            }
            UserSettings.JWTToken = await res.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", UserSettings.JWTToken);
        }

        public async Task GetPDF(int id, StorageFile file)
        {
            Uri source = new Uri($"{apiUrl}/Business/PdfForPromotion/{id}");

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, file);
            download.StartAsync();
        }

        public async Task<bool> PostSubscribeToBusiness(int businessId)
        {
            var json = JsonConvert.SerializeObject(businessId);
            var res = await client.PostAsync($"{apiUrl}/Account/Subscribe", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                throw new ArgumentException("Error Posting To Server");
            }
            var stringContent = await res.Content.ReadAsStringAsync();
            return bool.Parse(stringContent);
        }

        public async Task<bool> GetUserBusinessForSubscription(int businessId)
        {
            var json = JsonConvert.SerializeObject(businessId);
            var res = await client.PostAsync($"{apiUrl}/Account/CheckUserBusinessForSubscription", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

            if (!res.IsSuccessStatusCode)
            {
                throw new ArgumentException("Error Getting From Server");
            }
            var stringContent = await res.Content.ReadAsStringAsync();
            return bool.Parse(stringContent);
        }

        public async Task<List<PromotionViewModel>> GetPromotionsFromBusinessFromJWTAsync()
        {
            if (!UserSettings.IsLoggedIn)
            {
                throw new ArgumentException("Not logged In");
            }

            var jsonPromotion = await client.GetStringAsync($"{apiUrl}/Account/GetPromotionsFromAbonnees");
            return JsonConvert.DeserializeObject<List<PromotionViewModel>>(jsonPromotion);
        }

        public async Task<List<EventViewModel>> GetEventsFromBusinessFromJWTAsync()
        {
            if (!UserSettings.IsLoggedIn)
            {
                throw new ArgumentException("Not logged In");
            }

            var jsonEvent = await client.GetStringAsync($"{apiUrl}/Account/GetEventsFromAbbonees");
            return JsonConvert.DeserializeObject<List<EventViewModel>>(jsonEvent);
        }

        public async Task<BusinessViewModel> GetBusinessFromJWTAsync()
        {
            if (!UserSettings.IsLoggedIn)
            {
                throw new ArgumentException("Not logged In");
            }

            var token = UserSettings.JWTToken;

            var claims = JWTTokenConverter.ConvertToList(token);
            var businessId = claims.First(c => c.Type == "businessId").Value;
            var json = await client.GetStringAsync($"{apiUrl}/Business/Index/{businessId}");
            return JsonConvert.DeserializeObject<BusinessViewModel>(json);
        }

        public async Task PostBusinessFromJWTAsync(BusinessViewModel businessViewModel)
        {
            if (!UserSettings.IsLoggedIn)
            {
                throw new ArgumentException("Not logged In");
            }

            var token = UserSettings.JWTToken;

            var json = JsonConvert.SerializeObject(businessViewModel);
            var res = await client.PostAsync($"{apiUrl}/Business/Edit", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            if (!res.IsSuccessStatusCode)
            {
                throw new ArgumentException("Error Posting To Server");
            }
        }

        public async Task<List<BusinessViewModel>> GetPlacesAsync()
        {
            var json = await client.GetStringAsync($"{apiUrl}/Business/Index");
            var settings = new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.DateTimeOffset
            };
            return JsonConvert.DeserializeObject<List<BusinessViewModel>>(json, settings);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Windows_UWP.ViewModels
{
    public class UserBusinessViewModel
    {
        public int BusinessId { get; set; }

        public bool substatus;

        public string apiUrl = "http://localhost:5000/Account";

        public async Task Subscribe()
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(BusinessId);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/Subscribe", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                if (res.IsSuccessStatusCode)
                {
                    var stringContent = await res.Content.ReadAsStringAsync();
                    substatus = bool.Parse(stringContent);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task CheckUserBusinessForSubscribtion()
        {
            try
            {
                var token = ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken;


                var json = JsonConvert.SerializeObject(BusinessId);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                var res = await client.PostAsync($"{apiUrl}/CheckUserBusinessForSubscribtion", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                if (res.IsSuccessStatusCode)
                {
                    var stringContent = await res.Content.ReadAsStringAsync();
                    substatus = bool.Parse(stringContent);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

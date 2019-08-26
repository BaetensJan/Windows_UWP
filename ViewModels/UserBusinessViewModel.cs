using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows_UWP.Data;

namespace Windows_UWP.ViewModels
{
    public class UserBusinessViewModel
    {
        public int BusinessId { get; set; }

        public bool SubStatus { get; set; }

        private UserSettings UserSettings { get; set; }

        public UserBusinessViewModel()
        {
            UserSettings = (UserSettings)Application.Current.Resources["UserSettings"];
        }

        public async Task Subscribe()
        {
            try
            {
                if (UserSettings.IsLoggedIn)
                    SubStatus = await ApiClient.Instance.PostSubscribeToBusiness(BusinessId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task CheckUserBusinessForSubscription()
        {
            try
            {
                if (UserSettings.IsLoggedIn)
                    SubStatus = await ApiClient.Instance.GetUserBusinessForSubscription(BusinessId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

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

        public async Task Subscribe()
        {
            try
            {
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
                SubStatus = await ApiClient.Instance.GetUserBusinessForSubscription(BusinessId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

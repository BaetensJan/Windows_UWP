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
using Windows_UWP.ViewModels;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI;

namespace Windows_UWP.Views
{

    public sealed partial class LoginView : Page
    {

        public LoginViewModel LoginViewModel { get; set; } = new LoginViewModel();
        public List<PromotionViewModel> PromotionViewModel { get; set; } = new List<PromotionViewModel>();
        public List<EventViewModel> EventViewModel { get; set; } = new List<EventViewModel>();

        private const string apiUrl = "http://localhost:5000/account";

        public LoginView()
        {
            this.InitializeComponent();
        }
        private async void SubmitOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailTextBox.ClearValue(TextBox.BorderBrushProperty);
                PasswordTextBox.ClearValue(TextBox.BorderBrushProperty);

                var userJson = JsonConvert.SerializeObject(LoginViewModel);
                HttpClient client = new HttpClient();
                var res = await client.PostAsync($"{apiUrl}/login", new StringContent(userJson, System.Text.Encoding.UTF8, "application/json"));
                if (!res.IsSuccessStatusCode)
                {
                    throw new ArgumentException("Error whilst logging in!");
                }
                ((UserSettings)Application.Current.Resources["UserSettings"]).JWTToken = await res.Content.ReadAsStringAsync();

                var jsonPromotion = await client.GetStringAsync($"{apiUrl}/GetPromotionsFromAbonnees/{LoginViewModel.Email}");
                PromotionViewModel = JsonConvert.DeserializeObject<List<PromotionViewModel>>(jsonPromotion);
                var jsonEvent = await client.GetStringAsync($"{apiUrl}/GetEventsFromAbbonees/{LoginViewModel.Email}");
                EventViewModel = JsonConvert.DeserializeObject<List<EventViewModel>>(jsonEvent);

                CreatePromotionNotification();
                CreateEventsNotification();
                Frame.Navigate(typeof(PlacesView));
                this.Frame.BackStack.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Notification.Show(5000);
                EmailTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                PasswordTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                LoginViewModel.Email = "";
            }
        }

        private void CreateAccountOnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterView));
        }

        private void CreatePromotionNotification()
        {
                foreach (var promotion in PromotionViewModel)
                {
                ToastVisual visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Promotion name:" +  promotion.Name
                            },
                            new AdaptiveText()
                            {
                                Text = "Begins:" + promotion.StartDate.ToString()
                            },
                            new AdaptiveText()
                            {
                                Text = "Ends:" + promotion.EndDate.ToString()
                            },
                            new AdaptiveText()
                            {
                                Text = promotion.PromotionType.ToString()
                            }
                        }
                        }

                    };
                    createNotification(visual, promotion.Name);

                }
        }
        private void CreateEventsNotification()
        { 
            foreach (var xEvent in EventViewModel)
                    {
                        ToastVisual visual = new ToastVisual()
                        {
                            BindingGeneric = new ToastBindingGeneric()
                            {
                                Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Event name" + xEvent.Name
                            },
                            new AdaptiveText()
                            {
                                Text = xEvent.Type
                            },
                            new AdaptiveText()
                            {
                                Text = xEvent.Description
                            }
                        }
                            }
                        };
                        createNotification(visual, xEvent.Name);
                }

        }
        private void createNotification(ToastVisual visual, string name)
        {
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Launch = ""
            };
            
            // And create the toast notification
            var toast = new ToastNotification(toastContent.GetXml());
            toast.Group = LoginViewModel.Email;
            toast.Tag = name;
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}

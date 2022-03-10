using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Covid19.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
namespace Covid19.ViewModels
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public static ObservableCollection<NotificationPageModel> _notificationData;
        string existingCountry;
        string[] favCountry;
        CountryModel objCountryModel;
        Label lblNotificationStatus;
        ListView NotificationsListView;
        public NotificationViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            lblNotificationStatus = new Label();
            NotificationsListView = new ListView();
        }

        public NotificationViewModel()
        {}
        public static async void GetDeviceId(string DeviceId)//"5a4a83a0-4c4e-4843-b51a-9c124b191cbf"
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
              new KeyValuePair<string, string>("DeviceId", DeviceId)
            });

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            try
            {
                var responseMessage = httpClient.PostAsync("Https://www.santoshjayswal.com/api/covid", formContent).Result;
            }
            catch (Exception ex)
            {
                // Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
            }
        }

        public async void GetNotificationData(Label lbl, ListView listView)
        {
            string DeviceId = StaticClass.DeviceId;
            using (var client = new HttpClient())
            {

                var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("DeviceId", DeviceId)
            });
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync("https://www.santoshjayswal.com/api/covid/getnotification", formContent) ;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    var NotificationList = JsonConvert.DeserializeObject<List<NotificationPageModel>>(content);
                    notificationList = new ObservableCollection<NotificationPageModel>(NotificationList);
                    if (notificationList.Count > 0)
                    {
                        listView.IsVisible = true;
                        lbl.IsVisible = false;
                    }
                    else
                    {
                        //listView.IsVisible = false;
                        listView.BackgroundColor = Color.Transparent;
                        listView.Margin = new Thickness(0,-200,0,0);
                        lbl.IsVisible = true;
                        lbl.Text = "No Notifications found. Please come back later.";
                    }

                    
                }
                IsRefreshing = false;
            }
        }

    public Command RefreshData
        {
            get
            {
                return new Command(() =>
                {
                    GetNotificationData( lblNotificationStatus, NotificationsListView);
                });
            }
        }


        bool _isRefreshing;
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
        NotificationPageModel _selectedNotification;
        public NotificationPageModel SelectedNotification
        {
            get
            {
                return _selectedNotification;
            }
            set
            {
                _selectedNotification = value;
                if (value != null)
                {
                    value.Action = "ONE";
                    UpdateNotificationStatus(value);
                    //_selectedNotification.IsReadType = FontAttributes.None;
                }
                OnPropertyChanged();
            }
        }

        ObservableCollection<NotificationPageModel> _notificationList;
        public ObservableCollection<NotificationPageModel> notificationList
        {
            get
            {
                return _notificationList;
            }
            set
            {
                _notificationList = value;
                OnPropertyChanged();
            }
        }

        public  event PropertyChangedEventHandler PropertyChanged;
        protected  virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void UpdateNotificationStatus(NotificationPageModel value)
        {
            string DeviceId = StaticClass.DeviceId;

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("DeviceId", DeviceId),
                new KeyValuePair<string, string>("Action", value.Action),
                new KeyValuePair<string, string>("NotificationId", value.NotificationId.ToString())
            });

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var responseMessage = httpClient.PostAsync(StaticClass.UrlUpdateStatus, formContent).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                if (value.Action == "ONE")
                    await Navigation.PushAsync(new MainPage(objCountryModel, value.Country));
            }
        }
    }
}

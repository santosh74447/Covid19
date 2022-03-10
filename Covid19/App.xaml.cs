using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Covid19.Models;
using Covid19.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Covid19
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if(StaticClass.IsNotificationPage=="false")
                MainPage = new NavigationPage(new TotalPage());
            else
                MainPage = new NavigationPage(new NotificationPage());
            StaticClass.IsNotificationPage = "false";
            //MainPage = new NavigationPage(new NotificationPage());
            OneSignal.Current.StartInit("255da912-725f-46ad-80eb-e2fc3d6505f7")
            .HandleNotificationOpened(HandleNotificationOpened)
            .HandleNotificationReceived(HandleNotificationReceived)
            .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                .EndInit();
           // string s  = RegionInfo.CurrentRegion.GeoId.ToString();
            SomeMethod(); 
            SaveNewDevice();
        }

        public string Country { get; }
        void SomeMethod()
        {
            OneSignal.Current.IdsAvailable(IdsAvailable);
        }

        private void IdsAvailable(string userID, string pushToken)
        {
            Covid19.Models.StaticClass.DeviceId = userID;
            StaticClass.DeviceId = userID;
            if (Device.RuntimePlatform == Device.iOS)
            {
                StaticClass.DeviceId = "b74cc6a0-880e-4525-bb1a-d6faa6f325f5";
            }
        }
        private void HandleNotificationReceived(OSNotification notification)
        {
            
        }

        private void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            StaticClass.NavigationPage = result.notification.payload.additionalData["page"].ToString();
            StaticClass.IsNotificationPage = "true";
            MainPage = new NavigationPage(new NotificationPage());

        }

        public async void SaveNewDevice()
        {
            string DeviceId = StaticClass.DeviceId;
            using (var client = new HttpClient())
            {

                var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("DeviceId", DeviceId)
            });
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync("https://www.santoshjayswal.com/api/covid/savenewdevice", formContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                }
               
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

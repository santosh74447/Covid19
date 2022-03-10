using System;
using System.Collections.Generic;
using System.Net.Http;
using Covid19.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Essentials;
using Covid19.ViewModels;
using Covid19.Services;
using System.Linq;
using Rg.Plugins.Popup.Services;
using System.Threading;

namespace Covid19.Views
{
    public partial class TotalPage : ContentPage
    {
        DateTime updatedDate;
        
        public TotalPage()
        {
            InitializeComponent();
            Title = "Covid-19 Total Cases";
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            BindingContext = new RefreshData(Navigation);
           
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                GetSkillsData();
            }
            else
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = true;
                DisplayAlert("Unknown", "Active Internet Connection is Required to display data", "OK");
            }
        }

        void RunOnStart()
        {
            
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    layoutMain.IsVisible = true;
                    layoutNoWifi.IsVisible = false;
                    GetSkillsData();
                }
                else if (current == NetworkAccess.Local)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Local", "Active Internet Connection is Required to display data", "OK");
                }
                else if (current == NetworkAccess.ConstrainedInternet)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("ConstrainedInternet", "Active Internet Connection is Required to display data", "OK");
                }
                else if (current == NetworkAccess.None)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("None", "Active Internet Connection is Required to display data", "OK");
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Unknown", "Active Internet Connection is Required to display data", "OK");
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Total Page", "Error Occured While Loading Data. Please Try Again.", "OK");
            }
        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    layoutMain.IsVisible = true;
                    layoutNoWifi.IsVisible = false;
                }
                else if (current == NetworkAccess.Local)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Local", "Active Internet Connection is Required to display data", "OK");
                }
                else if (current == NetworkAccess.ConstrainedInternet)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("ConstrainedInternet", "Active Internet Connection is Required to display data", "OK");
                }
                else if (current == NetworkAccess.None)
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("None", "Active Internet Connection is Required to display data", "OK");
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Unknown", "Active Internet Connection is Required to display data", "OK");
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Total Page", "Error Occured While Loading Data. Please Try Again.", "OK");
            }
        }

        public async void GetUnreadNotificationDataAsync()
        {
            string DeviceId = StaticClass.DeviceId;
            using (var client = new HttpClient())
            {

                var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("DeviceId", DeviceId)
            });
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(StaticClass.UrlUnreadNotification, formContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    var NotificationList = JsonConvert.DeserializeObject<List<NotificationPageModel>>(content);
                    StaticClass.UnreadData = NotificationList[0].newNotificationCount;
                    DependencyService.Get<IBadgeService>().SetBadge(this, btnNotification, $"{StaticClass.UnreadData}", Color.Red, Color.White);
                }
            }
        }


        public async void GetSkillsData()
        {
            using (var client = new HttpClient())
            {
                // send a GET request  
                var uri = "https://corona.lmao.ninja/v2/all";
                var result = await client.GetStringAsync(uri);
                result = "[" + result + "]";
                var SkillsList = JsonConvert.DeserializeObject<List<CountryModel>>(result);

                //converting miliseconds to date
                double ticks = double.Parse(SkillsList[0].updated);
                TimeSpan time = TimeSpan.FromMilliseconds(ticks);
                updatedDate = new DateTime(1970, 1, 1) + time;

                lblUpdatedDate.Text = "Data as of " + updatedDate.ToString();
                lblTotalCases.Text = SkillsList[0].cases;
                lblTotalDeath.Text = SkillsList[0].deaths;
                lblActiveCases.Text = SkillsList[0].active;
                lblTotalRecovered.Text = SkillsList[0].recovered;
                lblTodayCases.Text = SkillsList[0].todayCases;
                lblTodayDeath.Text = SkillsList[0].todayDeaths;
                lblAffectedCountry.Text = SkillsList[0].affectedCountries;


               // GetUnreadNotificationDataAsync();
            }
        }

        void btnSource_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }
        void btnCountry_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CountryView());
        }
        void btnFavourate_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new FavouratePage());
        }
        async void btnShare_Clicked(System.Object sender, System.EventArgs e)
        {
            await myScrollView.ScrollToAsync(0, lblInfo.Y, true);
            CommonModel.ShareFile();
        }
        public Command RefreshData
        {
            get
            {
                return new Command(() =>
                {
                    DisplayAlert("Refresh Command", "Hurray It worked", "OK");
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
            }
        }
        void btnNotification_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());

        }
        async void DisplayDefaultCountry()
        {
            
            if (Device.RuntimePlatform == "Android")
            {
                if (StaticClass.IsDefaultCountry == "")
                {
                    await PopupNavigation.Instance.PushAsync(new DefaultCountry());
                }
               // DependencyService.Get<IBadgeService>().SetBadge(this, btnNotification, $"{10}", Color.Red, Color.White);
                // await DisplayAlert("Default Country ", StaticClass.IsDefaultCountry + " added as default country to receive notification.", "OK");
            }
        }
        protected override void OnAppearing()
        {
            CommonModel.GetUnreadNotificationDataAsync(this,btnNotification);
            //GetUnreadNotificationDataAsync();
            //DependencyService.Get<IBadgeService>().SetBadge(this, btnNotification, $"{StaticClass.UnreadData}", Color.Red, Color.White);

            //DependencyService.Get<IBadgeService>().SetBadge(this, btnNotification, $"{10}", Color.Red, Color.White);

            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnAppearing();
        }

    }
}

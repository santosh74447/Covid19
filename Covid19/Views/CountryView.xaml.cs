using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Covid19.Models;
using Covid19.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19.Views
{
    public partial class CountryView : ContentPage
    {
        CountryViewModel countryViewModel = new CountryViewModel();
        string country, existingCountry;

        public CountryView()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    SkillsListView.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
                    BindingContext = new CountryViewModel(Navigation);
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Unknown Country", "Active Internet Connection is Required to display data", "OK");
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Country", "Error Occured While Loading Data. Please Try Again.", "OK");
            }
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                layoutMain.IsVisible = true;
                layoutNoWifi.IsVisible = false;
            }
            else
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    (this.BindingContext as CountryViewModel).GetCountryData(false);
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Country", "Error Occured While Loading Data. Please Try Again.", "OK");
            }
        }

        void txtSearch_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SkillsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                SkillsListView.ItemsSource = CountryViewModel._scollectionSkills;
            else
                SkillsListView.ItemsSource = CountryViewModel._scollectionSkills.Where(i => i.country.Contains(txtSearch.Text));

            SkillsListView.EndRefresh();
        }
        void imgBtnBookMark_Clicked(System.Object sender, System.EventArgs e)
        {

            //ImageButton button = (ImageButton)sender;
            //country = button.CommandParameter.ToString();


            //CommonModel.imgBtnBookMark_Clicked(this,sender,e);
            ImageButton button = (ImageButton)sender;
            //country = button.CommandParameter.ToString();
            CountryModel objCountry = button.BindingContext as Covid19.Models.CountryModel;
            //if (objCountry.IsBookmarkExist == true)
            //{
            //    objCountry.Action = "DELETE";
            //}
            //else
            //{
            //    objCountry.Action = "INSERT";
            //}
            //var formContent = new FormUrlEncodedContent(new[]
            //    {
            //         new KeyValuePair<string, string>("DeviceId", StaticClass.DeviceId),
            //         new KeyValuePair<string, string>("CountryName", objCountry.country),
            //         new KeyValuePair<string, string>("Action", objCountry.Action)
            //     });

            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //try
            //{
            //    var responseMessage = httpClient.PostAsync(StaticClass.UrlBookmarkedCountry, formContent).Result;
            //    if (responseMessage.IsSuccessStatusCode)
            //    {
            //        if (objCountry.IsBookmarkExist == true)
            //        {
            //            DisplayAlert("Favourate ", objCountry.country + " Removed from Bookmark List.", "OK");
            //            button.Source = "bmark.png";
            //            objCountry.IsBookmarkExist = false;
            //        }
            //        else
            //        {
            //            DisplayAlert("Bookmark ", objCountry.country + " Bookmarked.", "OK");
            //            button.Source = "fmark.png";
            //            objCountry.IsBookmarkExist = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DisplayAlert("Bookmark ", "Error Occured! Please try again.", "OK");
            //}


            //if (StaticClass.BookmarkedCountry == "0")
            //{
            //    StaticClass.BookmarkedCountry = country + ",";
            //    DisplayAlert("Favourate ", country + " Bookmarked.", "OK");
            //    button.Source = "fmark.png";
            //}
            //else
            //{
            //    StaticClass.BookmarkedCountry += country + ",";
            //    DisplayAlert("Favourate ", country + " Bookmarked.", "OK");
            //    button.Source = "fmark.png";
            //}

            if (Application.Current.Properties.ContainsKey("country"))
            {

                existingCountry = Application.Current.Properties["country"].ToString();
                if (!existingCountry.Contains(objCountry.country))
                {
                    if (Application.Current.Properties.ContainsKey("country"))
                    {
                        Application.Current.Properties["country"] += objCountry.country + ",";
                    }
                    DisplayAlert("Bookmark ", objCountry.country + " Bookmarked.", "OK");
                    button.Source = "fmark.png";
                }
                else
                {
                    existingCountry = existingCountry.Replace(objCountry.country + ",", "");
                    Application.Current.Properties["country"] = existingCountry;
                    DisplayAlert("Favourate ", country + " Removed from Bookmark List.", "OK");
                    button.Source = "bmark.png";
                }
            }
            else
            {
                Application.Current.Properties["country"] = country + ",";
                DisplayAlert("Favourate ", objCountry.country + " Bookmarked.", "OK");
                button.Source = "fmark.png";
            }
        }

        void btnShare_Clicked(System.Object sender, System.EventArgs e)
        {
            CommonModel.ShareFile();
        }
    }
}

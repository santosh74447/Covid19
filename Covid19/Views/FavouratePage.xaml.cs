using System;
using Covid19.Models;
using Covid19.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19.Views
{
    public partial class FavouratePage : ContentPage
    {
        public FavouratePage()
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
                    DisplayAlert("Unknown Favourate", "Active Internet Connection is Required to display data", "OK");
                    
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Favourate", "Error Occured While Loading Data. Please Try Again.", "OK");
                
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
               // DisplayAlert("Unknown Favourate", "Active Internet Connection is Required to display data", "OK");
                
            }
        }
        protected override void OnAppearing()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    (this.BindingContext as CountryViewModel).GetCountryData(true );
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

        void imgBtnBookMark_Clicked(System.Object sender, System.EventArgs e)
        {
            CommonModel.imgBtnBookMark_Clicked(this, sender, e);
            OnAppearing();
        }
        void btnShare_Clicked(System.Object sender, System.EventArgs e)
        {
            CommonModel.ShareFile();
        }
    }
}

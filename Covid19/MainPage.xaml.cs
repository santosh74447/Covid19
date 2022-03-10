using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using Covid19.Models;
using Covid19.ViewModels;
using Covid19.Views;
using Newtonsoft.Json;
using Plugin.Screenshot;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
         string country, existingCountry;
        bool isFavourate = false;
        CountryViewModel viewModel = new CountryViewModel();
        CountryModel countryModel = new CountryModel();
        public static System.Threading.Tasks.Task<ObservableCollection<CountryModel>> _scollectionSkills;

        public  MainPage(CountryModel SelectedSkills, string notificationCountry)
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            countryModel = SelectedSkills;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    layoutMain.IsVisible = true;
                    layoutNoWifi.IsVisible = false;
                    if (notificationCountry != "")
                    {
                        GetSpecificCountryData(notificationCountry);
                    }
                    else
                    {
                        Title = SelectedSkills.country.ToUpper();
                        country = SelectedSkills.country;
                        lblTodayCases.Text = SelectedSkills.todayCases;
                        lblTodayDeath.Text = SelectedSkills.todayDeaths;
                        lblTotalCases.Text = SelectedSkills.cases;
                        lblTotalRecovered.Text = SelectedSkills.recovered;
                        lblTotalDeath.Text = SelectedSkills.deaths;
                        lblActiveCases.Text = SelectedSkills.active;
                        lblCriticalCases.Text = SelectedSkills.critical;
                        lblCasesPerMillion.Text =  SelectedSkills.casesPerOneMillion;
                        lblDeathPerMillion.Text = SelectedSkills.deathsPerOneMillion;


                        //converting miliseconds to date
                        double ticks = double.Parse(SelectedSkills.updated);
                        TimeSpan time = TimeSpan.FromMilliseconds(ticks);
                        DateTime updatedDate = new DateTime(1970, 1, 1) + time;

                        lblUpdatedDate.Text = "Data as of " + updatedDate.ToString();

                        if (Application.Current.Properties.ContainsKey("country"))
                        {
                            existingCountry = Application.Current.Properties["country"].ToString();
                            if (existingCountry.Contains(country))
                            {
                                //btnFavourate.ImageSource = "fmark.png";
                                isFavourate = true;
                            }
                        }
                    }
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Unknown", "Active Internet Connection is Required to display data", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error Occured", "Error Occured While Loading Data. Please Try Again.", "OK");
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                return;
            }

        }
        async void GetSpecificCountryData(string notificationCountry)
        {
            using (var client = new HttpClient())
            {
                var uri = "";
                var result = "";
                uri = "https://corona.lmao.ninja/v2/countries/" + notificationCountry;
                result = await client.GetStringAsync(uri);
                result = "[" + result + "]";
                var SelectedSkills = JsonConvert.DeserializeObject<List<CountryModel>>(result);

                Title = SelectedSkills[0].country.ToUpper();
                country = SelectedSkills[0].country;
                lblTodayCases.Text = SelectedSkills[0].todayCases;
                lblTodayDeath.Text = SelectedSkills[0].todayDeaths;
                lblTotalCases.Text = SelectedSkills[0].cases;
                lblTotalRecovered.Text = SelectedSkills[0].recovered;
                lblTotalDeath.Text = SelectedSkills[0].deaths;
                lblActiveCases.Text = SelectedSkills[0].active;
                lblCriticalCases.Text = SelectedSkills[0].critical;
                lblCasesPerMillion.Text = SelectedSkills[0].casesPerOneMillion;
                lblDeathPerMillion.Text = SelectedSkills[0].deathsPerOneMillion;


                //converting miliseconds to date
                double ticks = double.Parse(SelectedSkills[0].updated);
                TimeSpan time = TimeSpan.FromMilliseconds(ticks);
                DateTime updatedDate = new DateTime(1970, 1, 1) + time;

                lblUpdatedDate.Text = "Data as of " + updatedDate.ToString();

                if (Application.Current.Properties.ContainsKey("country"))
                {
                    existingCountry = Application.Current.Properties["country"].ToString();
                    if (existingCountry.Contains(country))
                    {
                       // btnFavourate.ImageSource = "fmark.png";
                        isFavourate = true;
                    }
                }
            }
        }
        //protected override bool OnBackButtonPressed()
        //{
        //    if(StaticClass.NavigationPage!="TotalPage")
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //              App.Current.MainPage = new NavigationPage(new TotalPage());
        //        });
        //    return true;
        //}

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                layoutMain.IsVisible = true;
                layoutNoWifi.IsVisible = false;
                btnFavourate.IsEnabled = true ;
            }
            else
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = true;
                btnFavourate.IsEnabled = false;
                //DisplayAlert("Unknown", "Active Internet Connection is Required to display data", "OK");
                return;
            }
        }

            void btnSource_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AboutPage());
        }

        void btnHistory_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new GraphPage(country));
        }

        async void btnFavourate_Clicked(System.Object sender, System.EventArgs e)
        {
            btnFavourate.BindingContext = countryModel;
                 
            CommonModel.imgBtnBookMark_Clicked(this, sender, e);

            //if (isFavourate == false)
            //{
            //    if (Application.Current.Properties.ContainsKey("country"))
            //    {
            //        Application.Current.Properties["country"] += country + ",";
            //    }
            //    else
            //    {
            //        Application.Current.Properties["country"] = country + ",";
            //    }
            //    await DisplayAlert("Bookmark ", country + " Bookmarked.", "OK");
            //    btnFavourate.ImageSource = "fmark.png";
            //    isFavourate = true;
            //}
            //else
            //{
            //    existingCountry = existingCountry.Replace(country+",", "");
            //    Application.Current.Properties["country"] = existingCountry;
            //    await DisplayAlert("Favourate ", country + " Removed from Bookmark List.", "OK");
            //    btnFavourate.ImageSource = "bmark.png";
            //    isFavourate = false;
            //}
        }

        void btnShare_Clicked(System.Object sender, System.EventArgs e)
        {
            CommonModel.ShareFile();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Covid19.Models;
using Covid19.ViewModels;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Covid19.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultCountry : PopupPage
    {
        DefaultCountryViewModel countryViewModel = new DefaultCountryViewModel();
        public static ObservableCollection<CountryModel> _scollectionSkills;

        public DefaultCountry()
        {
            InitializeComponent();
            SkillsListView.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
            BindingContext = new DefaultCountryViewModel(Navigation);

        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        void txtSearch_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SkillsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                SkillsListView.ItemsSource = DefaultCountryViewModel._scollectionSkills;
            else
                SkillsListView.ItemsSource = DefaultCountryViewModel._scollectionSkills.Where(i => i.country.Contains(txtSearch.Text));

            SkillsListView.EndRefresh();
        }
        protected override void OnAppearing()
        {
            try
            {
                (this.BindingContext as DefaultCountryViewModel).GetSkillsData(false);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error Occured Country", "Error Occured While Loading Data. Please Try Again.", "OK");

            }
        }
        protected override void OnDisappearing()
        {
            DisplayAlert("Default Country ", StaticClass.IsDefaultCountry + " added as default country to receive notification.", "OK");
            base.OnDisappearing();
        }

    }
}

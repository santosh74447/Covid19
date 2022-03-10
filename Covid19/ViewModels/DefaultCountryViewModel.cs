using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Covid19.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
namespace Covid19.ViewModels
{
    public class DefaultCountryViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public static ObservableCollection<CountryModel> _scollectionSkills;
        string existingCountry;
        string[] favCountry;
        public DefaultCountryViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public DefaultCountryViewModel()
        {

        }
        public async void GetSkillsData(bool isFav)
        {
            using (var client = new HttpClient())
            {
                var uri = "";
                var result = "";
                var SkillsList = JsonConvert.DeserializeObject<List<CountryModel>>(result);
                // send a GET request
                if (isFav == false)
                {
                    uri = "https://corona.lmao.ninja/v2/countries?sort=cases";
                    result = await client.GetStringAsync(uri);
                    SkillsList = JsonConvert.DeserializeObject<List<CountryModel>>(result);
                    collectionSkills = new ObservableCollection<CountryModel>(SkillsList);
                    _scollectionSkills = collectionSkills;
                    IsRefreshing = false;
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("country"))
                    {
                        existingCountry = Application.Current.Properties["country"].ToString();
                        if (existingCountry.Length > 0)
                        {
                            existingCountry = existingCountry.Substring(0, existingCountry.Length - 1);
                            favCountry = existingCountry.Split(',');
                            foreach (var item in favCountry)
                            {
                                uri = "https://corona.lmao.ninja/v2/countries/" + item.ToString();
                                result += await client.GetStringAsync(uri) + ",";
                            }
                            result = result.Substring(0, result.Length - 1);
                            result = "[" + result + "]";
                            SkillsList = JsonConvert.DeserializeObject<List<CountryModel>>(result);

                            collectionSkills = new ObservableCollection<CountryModel>(SkillsList);
                            _scollectionSkills = collectionSkills;
                            IsRefreshing = false;
                        }
                        else
                        {
                            collectionSkills = null;
                        }

                    }

                }
            }
        }
        public Command RefreshData
        {
            get
            {
                return new Command(() =>
                {
                    GetSkillsData(false);
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
        CountryModel _selectedSkills;
        public CountryModel SelectedSkills
        {
            get
            {
                return _selectedSkills;
            }
            set
            {
                _selectedSkills = value;
                if (value != null)
                {
                    StaticClass.IsDefaultCountry = SelectedSkills.country;
                    Application.Current.Properties["country"] = StaticClass.IsDefaultCountry + ",";
                    PopupNavigation.Instance.PopAsync(true);
                    //Page.DisplayAlert("Default Country ", SelectedSkills.country + " added as default country to receive notification.", "OK");

                }
                OnPropertyChanged();
            }
        }

        ObservableCollection<CountryModel> _collectionSkills;
        public ObservableCollection<CountryModel> collectionSkills
        {
            get
            {
                return _collectionSkills;
            }
            set
            {
                _collectionSkills = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
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
    public class CountryViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public static ObservableCollection<CountryModel> _scollectionSkills;
        string existingCountry;
        string[] favCountry;
        public CountryViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
        }

        public CountryViewModel()
        {
            
        }
        public async void GetCountryData(bool isFav)
        {
            //GetBookMarkedCountry();
            using (var client = new HttpClient())
            {
                var uri = "";
                var result="";
                var SkillsList = JsonConvert.DeserializeObject<List<CountryModel>>(result);
                // send a GET request
                if (isFav == false)
                {
                    uri = StaticClass.UrlCovidCountry;
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
                                uri = StaticClass.UrlCovidSpecificCountry + item.ToString();
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

        public async void GetBookMarkedCountry()
        {
            string DeviceId = StaticClass.DeviceId;
            using (var client = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("DeviceId", DeviceId),
                    new KeyValuePair<string, string>("Action", "ALL")
                });
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(StaticClass.UrlGetBookmarkedData, formContent);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsStringAsync();
                    var bookmarkedCountry = JsonConvert.DeserializeObject<List<CountryModel>>(content);
                    if (bookmarkedCountry.Count > 0)
                    {
                        Application.Current.Properties["country"] = "";
                        foreach (var item in bookmarkedCountry)
                        {
                            Application.Current.Properties["country"] += item.countryName + ",";
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
                    GetCountryData(false);
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
                     Navigation.PushAsync(new MainPage(SelectedSkills,""));
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
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

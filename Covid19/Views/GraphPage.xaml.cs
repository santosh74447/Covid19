using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Covid19.Models;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Essentials;
using Entry = Microcharts.ChartEntry;
using Plugin.Screenshot;

namespace Covid19.Views
{
    public partial class GraphPage : ContentPage
    {
        public GraphPage(string country)
        {
            InitializeComponent();
            Title = "Graphical Representation (" + country+")";
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    GetSkillsData(country);
                }
                else
                {
                    layoutMain.IsVisible = false;
                    layoutNoWifi.IsVisible = true;
                    DisplayAlert("Unknown Graph", "Active Internet Connection is Required to display data", "OK");
                    
                }
            }
            catch (Exception ex)
            {
                layoutMain.IsVisible = false;
                layoutNoWifi.IsVisible = false;
                DisplayAlert("Error Occured Graph", "Error Occured While Loading Data. Please Try Again.", "OK");
                
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
               // DisplayAlert("Unknown Graph", "Active Internet Connection is Required to display data", "OK");
                
            }
        }
        public async void GetSkillsData(string country)
        {
            using (var client = new HttpClient())
            {
                if (country == "USA")
                    country = "usa";
                // send a GET request  
                var uri = "https://corona.lmao.ninja/v2/historical/"+country;
                var result = await client.GetStringAsync(uri);
                result = result.Replace("}", "");
                //result = result.Substring(44, result.Length - 47).ToString();
                string[] s = result.Split(',');
                int index = 0;
                int rIndex=0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Contains("deaths"))
                    {
                        index = i;
                        break;
                    }
                }

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].Contains("recovered"))
                    {
                        rIndex = i;
                        break;
                    }
                }

                // char[] MyChar = { '"', '{', '}' };
                //result = result.TrimEnd(MyChar);

                // result = result.Remove('"');
                string[] date = new string[10];
                string[] value = new string[10];
                string[] deathDate = new string[10];
                string[] deatthValue = new string[10];
                string[] recoveredDate = new string[10];
                string[] recoveredValue = new string[10];
                int c = 0;
                for (int i = index - 11; i < index-1; i++)
                {
                    string[] m = s[i].Split(':');
                    string[] n = s[i + 1].Split(':');
                    date[c] = (n[0]);
                    value[c] = (Convert.ToInt32(n[1]) - Convert.ToInt32(m[1])).ToString();
                    c++;
                }
                 c = 0;
                for (int i = rIndex - 11; i < rIndex-1; i++)
                {
                    string[] m = s[i].Split(':');
                    string[] n = s[i+1].Split(':');
                    deathDate[c] = (n[0]);
                    deatthValue[c] = (Convert.ToInt32(n[1])- Convert.ToInt32(m[1])).ToString();
                    c++;
                }
                c = 0;
                for(int i = s.Length - 11; i < s.Length-1; i++)
                {
                    string[] m = s[i].Split(':');
                    string[] n = s[i + 1].Split(':');
                    recoveredDate[c] = (n[0]);
                    recoveredValue[c] = (Convert.ToInt32(n[1]) - Convert.ToInt32(m[1])).ToString();
                    c++;
                }

                List<Entry> entries = new List<Entry>
                {
                };
                List<Entry> deathData = new List<Entry>
                {
                };
                List<Entry> recoveredData = new List<Entry>
                {
                };
                for (int i = 0; i < date.Length; i++)
                {
                    if (i % 2 == 0)
                        entries.Add(new Entry(Convert.ToInt32(value[i]))
                        {
                            Color = SKColor.Parse("#FF1943"),
                            Label = date[i].Replace('"', ' '),
                            ValueLabel = value[i],
                        });
                    else
                        entries.Add(new Entry(Convert.ToInt32(value[i]))
                        {
                            Color = SKColor.Parse("#68B9C0"),
                            Label = date[i].Replace('"', ' '),
                            ValueLabel = value[i],
                        });
                }
                for (int i = 0; i < deathDate.Length; i++)
                {
                    if (i % 2 == 0)
                        deathData.Add(new Entry(Convert.ToInt32(deatthValue[i]))
                        {
                            Color = SKColor.Parse("#FF1943"),
                            Label = date[i].Replace('"', ' '),
                            ValueLabel = deatthValue[i],
                        });
                    else
                        deathData.Add(new Entry(Convert.ToInt32(deatthValue[i]))
                        {
                            Color = SKColor.Parse("#68B9C0"),
                            Label = deathDate[i].Replace('"', ' '),
                            ValueLabel = deatthValue[i],
                        });
                }
                for (int i = 0; i < recoveredDate.Length; i++)
                {
                    if (i % 2 == 0)
                        recoveredData.Add(new Entry(Convert.ToInt32(recoveredValue[i]))
                        {
                            Color = SKColor.Parse("#FF1943"),
                            Label = recoveredDate[i].Replace('"', ' '),
                            ValueLabel = recoveredValue[i],
                        });
                    else
                        recoveredData.Add(new Entry(Convert.ToInt32(recoveredValue[i]))
                        {
                            Color = SKColor.Parse("#68B9C0"),
                            Label = recoveredDate[i].Replace('"', ' '),
                            ValueLabel = recoveredValue[i],
                        });
                }
                Chart1.Chart = new LineChart() { Entries = entries };
                ChartDeath.Chart = new LineChart() { Entries = deathData };
                ChartRecovered.Chart = new LineChart() { Entries = recoveredData };
                //_scollectionSkills = collectionSkills;
                //IsRefreshing = false;
            }
        }
        ObservableCollection<CountryGraphModel> _collectionSkills;
        public ObservableCollection<CountryGraphModel> collectionSkills
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

        void btnShare_Clicked(System.Object sender, System.EventArgs e)
        {
            CommonModel.ShareFile();
        }
    }
}

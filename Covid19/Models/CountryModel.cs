using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;

namespace Covid19.Models
{
    public class CountryModel
    {
        private string _country;
        public string country { get=>_country;

            set
            {
                _country = value;

                if (Application.Current.Properties.ContainsKey("country"))
                {
                    string existingCountry = Application.Current.Properties["country"].ToString();
                    if (existingCountry.Contains(value))
                    {
                        markIcon = "fmark.png";
                        IsBookmarkExist = true;
                    }
                    else
                    {
                        markIcon = "bmark.png";
                        IsBookmarkExist = false;
                    }

                }
                else
                {
                    markIcon = "bmark.png";
                    IsBookmarkExist = false;
                }
            }



        }
        public bool IsBookmarkExist { get; set; }
        public string Action { get; set; }
        public string countryName { get; set; }
        public string markIcon { get; set; }
        public countryInfo countryInfo { get; set; }
        private string _cases;
        public string cases
        {
            get => _cases;
            set
            {
                _cases = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value));
            }
        }

        private string _todayCases;
        public string todayCases { get => _todayCases; set { _todayCases = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _deaths;
        public string deaths { get => _deaths; set { _deaths = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _todayDeaths;
        public string todayDeaths { get => _todayDeaths; set { _todayDeaths = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _recovered;
        public string recovered { get => _recovered; set { _recovered = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _active;
        public string active { get => _active; set { _active = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _critical;
        public string critical { get => _critical; set { _critical = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        private string _casesPerOneMillion;
        public string casesPerOneMillion { get => _casesPerOneMillion; set { _casesPerOneMillion = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }


        private string _deathsPerOneMillion;
        public string deathsPerOneMillion { get => _deathsPerOneMillion; set { _deathsPerOneMillion = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

        public string updated { get; set; }

        private string _affectedCountries;
        public string affectedCountries { get => _affectedCountries; set { _affectedCountries = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", Convert.ToDouble(value)); ; } }

       
    }

    public class countryInfo
    {
        public string _id { get; set; }
        public string lat { get; set; }
        // public string long { get; set; }
        public string flag { get; set; }
        public string iso3 { get; set; }
        public string iso2 { get; set; }
    }
}
    

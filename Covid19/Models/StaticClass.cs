using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Covid19.Models
{
    public class StaticClass
    {
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }
        //localhost url
        //public static string UrlUnreadNotification = "http://192.168.1.107/covid/api/covid/getunreadnotification";
        //public static string UrlUpdateStatus = "http://192.168.1.107/covid/api/covid/updatestatus";
        //public static string UrlBookmarkedCountry = "http://192.168.1.107/covid/api/covid/savebookmarkedcountry";
        //public static string UrlGetBookmarkedData = "http://192.168.1.107/covid/api/covid/getbookmarkeddata";


        //final url
        public static string UrlUnreadNotification = "https://www.santoshjayswal.com/api/covid/getunreadnotification";
        public static string UrlUpdateStatus = "https://www.santoshjayswal.com/api/covid/updatestatus";
        public static string UrlBookmarkedCountry = "https://www.santoshjayswal.com/api/covid/savebookmarkedcountry";
        public static string UrlGetBookmarkedData = "https://www.santoshjayswal.com/api/covid/getbookmarkeddata";
        public static string UrlCovidCountry ="https://corona.lmao.ninja/v2/countries?sort=cases";
        public static string UrlCovidSpecificCountry = "https://corona.lmao.ninja/v2/countries/";


        private const string NavigationPageKey = "NavigationPage_key";
        private static readonly string NavigationPageDefault = "TotalPage";
        public static string NavigationPage
        {
            get { return AppSettings.GetValueOrDefault(NavigationPageKey, NavigationPageDefault); }
            set { AppSettings.AddOrUpdateValue(NavigationPageKey, value); }
        }

        private const string WritePermissionKey = "WritePermission_key";
        private static readonly string WritePermissionDefault = "Denied";
        public static string WritePermission
        {
            get { return AppSettings.GetValueOrDefault(WritePermissionKey, WritePermissionDefault); }
            set { AppSettings.AddOrUpdateValue(WritePermissionKey, value); }
        }
        private const string DeviceIdKey = "DeviceId_key";
        private static readonly string DeviceIdDefault = "";
        public static string DeviceId
        {
            get { return AppSettings.GetValueOrDefault(DeviceIdKey, DeviceIdDefault); }
            set { AppSettings.AddOrUpdateValue(DeviceIdKey, value); }
        }
        private const string IsNotificationPageKey = "IsNotificationPage_key";
        private static readonly string IsNotificationPageDefault = "false";
        public static string IsNotificationPage
        {
            get { return AppSettings.GetValueOrDefault(IsNotificationPageKey, IsNotificationPageDefault); }
            set { AppSettings.AddOrUpdateValue(IsNotificationPageKey, value); }
        }
        private const string IsDefaultCountryKey = "IsDefaultCountry_key";
        private static readonly string IsDefaultCountryDefault = "";
        public static string IsDefaultCountry
        {
            get { return AppSettings.GetValueOrDefault(IsDefaultCountryKey, IsDefaultCountryDefault); }
            set { AppSettings.AddOrUpdateValue(IsDefaultCountryKey, value); }
        }

        private const string UnreadDataKey = "UnreadData_key";
        private static readonly string UnreadDataDefault = "0";
        public static string UnreadData
        {
            get { return AppSettings.GetValueOrDefault(UnreadDataKey, UnreadDataDefault); }
            set { AppSettings.AddOrUpdateValue(UnreadDataKey, value); }
        }


        private const string BookmarkedCountryDataKey = "BookmarkedCountry_key";
        private static readonly string BookmarkedCountryDataDefault = "0";
        public static string BookmarkedCountry
        {
            get { return AppSettings.GetValueOrDefault(BookmarkedCountryDataKey, BookmarkedCountryDataDefault); }
            set { AppSettings.AddOrUpdateValue(BookmarkedCountryDataKey, value); }
        }

    }
}

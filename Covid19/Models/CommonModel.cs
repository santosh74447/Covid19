using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Covid19.Services;
using Newtonsoft.Json;
using Plugin.Screenshot;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19.Models
{
    public static class CommonModel
    {
        public static async void CheckPermission()
        {

            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }

        }
        public static async void GetUnreadNotificationDataAsync(Page page, ToolbarItem toolbarItem)
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
                    DependencyService.Get<IBadgeService>().SetBadge(page, toolbarItem, $"{StaticClass.UnreadData}", Color.Red, Color.White);
                }
            }
        }


        public static void imgBtnBookMark_Clicked(Page page, System.Object sender, System.EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            //country = button.CommandParameter.ToString();
            CountryModel objCountry = button.BindingContext as Covid19.Models.CountryModel;
            if (objCountry.IsBookmarkExist == true)
            {
                objCountry.Action = "DELETE";
            }
            else
            {
                objCountry.Action = "INSERT";
            }
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
            //            page.DisplayAlert("Favourate ", objCountry.country + " Removed from Bookmark List.", "OK");
            //            button.Source = "bmark.png";
            //            objCountry.IsBookmarkExist = false;
            //        }
            //        else
            //        {
            //            page.DisplayAlert("Bookmark ", objCountry.country + " Bookmarked.", "OK");
            //            button.Source = "fmark.png";
            //            objCountry.IsBookmarkExist = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    page.DisplayAlert("Bookmark ", "Error Occured! Please try again.", "OK");
            //}
        }



        public static async void ShareFile()
        {
            if (StaticClass.WritePermission == "Granted")
            {
                string path = await CrossScreenshot.Current.CaptureAndSaveAsync();
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Covid19",
                    File = new ShareFile(path)
                });
            }
            else
            {
                CommonModel.CheckPermission();
            }
            //to display data without saving to device taken using screenshot features.
            //var stream = new MemoryStream(await CrossScreenshot.Current.CaptureAsync());
            //imgCaptured.Source = ImageSource.FromStream(() => stream);
        }
    }
}

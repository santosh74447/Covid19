using System;
using System.Collections.Generic;
using Covid19.Models;
using Covid19.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Covid19.Views
{
    public partial class NotificationPage : ContentPage
    {
        NotificationViewModel objNotificationViewModel = new NotificationViewModel();
        NotificationPageModel objNotification = new NotificationPageModel();
        public NotificationPage()
        {
            InitializeComponent();
            try
            {
                NotificationsListView.HeightRequest = DeviceDisplay.MainDisplayInfo.Height;
                BindingContext = new NotificationViewModel(Navigation);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Alert", "No internet connection", "OK");
                });

            }
        }
        protected override void OnAppearing()
        {
            try
            {
                (this.BindingContext as NotificationViewModel).GetNotificationData(lblNotificationStatus, NotificationsListView);
            }
            catch (System.Net.Http.HttpRequestException)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Alert", "No internet connection", "Ok");
                });

            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                App.Current.MainPage = new NavigationPage(new TotalPage());
            });
            return true;
        }

        void btnHome_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new TotalPage());
        }

        async void btnOptions_Clicked(System.Object sender, System.EventArgs e)
        {
            string action = await DisplayActionSheet("Manage Notification!", "Cancel",null, "Mark All as Read", "Delete All Notification");
            //Debug.WriteLine("Action: " + action);
            if(action== "Mark All as Read")
            {
                objNotification.Action = "ALL";
                objNotificationViewModel.UpdateNotificationStatus(objNotification);
            }
            else if (action== "Delete All Notification")
            {
                objNotification.Action = "DELETE";
                objNotificationViewModel.UpdateNotificationStatus(objNotification);
            }
            OnAppearing();
        }
    }
}

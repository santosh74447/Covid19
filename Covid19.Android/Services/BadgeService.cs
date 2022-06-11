using System;
using Covid19.Droid.Services;
using Covid19.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(BadgeService))]
namespace Covid19.Droid.Services
{
    public class BadgeService : IBadgeService
    {
        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                //var toolbar = CrossCurrentActivity.Current.Activity.FindViewById(Resource.Id.toolbar) as Android.Support.V7.Widget.Toolbar;
                //if (toolbar != null)
                //{
                //    if (!string.IsNullOrEmpty(value))
                //    {
                //        var idx = page.ToolbarItems.IndexOf(item);
                //        if (toolbar.Menu.Size() > idx)
                //        {
                //            var menuItem = toolbar.Menu.GetItem(idx);
                //            BadgeDrawable.SetBadgeText(CrossCurrentActivity.Current.Activity, menuItem, value, backgroundColor.ToAndroid(), textColor.ToAndroid());
                //        }
                //    }
                //}
            });
        }
    }
}

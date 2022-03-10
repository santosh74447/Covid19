using System;
using Covid19.iOS.Utils;
using Covid19.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Covid19.iOS.Services
{
    public class BadgeService : IBadgeService
    {
        public void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var renderer = Platform.GetRenderer(page);
                if (renderer == null)
                {
                    renderer = Platform.CreateRenderer(page);
                    Platform.SetRenderer(page, renderer);
                }
                var vc = renderer.ViewController;

                var rightButtonItems = vc?.ParentViewController?.NavigationItem?.RightBarButtonItems;

                // If we can't find the button where it typically is check the child view controllers
                // as this is where MasterDetailPages are kept
                if (rightButtonItems == null && vc.ChildViewControllerForHomeIndicatorAutoHidden != null)
                    foreach (var uiObject in vc.ChildViewControllerForHomeIndicatorAutoHidden)
                    {
                        string uiObjectType = uiObject.GetType().ToString();

                        if (uiObjectType.Contains("FormsNav"))
                        {
                            UIKit.UINavigationBar navobj = (UIKit.UINavigationBar)uiObject;

                            if (navobj.Items != null)
                                foreach (UIKit.UINavigationItem navitem in navobj.Items)
                                {
                                    if (navitem.RightBarButtonItems != null)
                                    {
                                        rightButtonItems = navitem.RightBarButtonItems;
                                        break;
                                    }
                                }
                        }
                    }

                var idx = page.ToolbarItems.IndexOf(item);
                if (rightButtonItems != null && rightButtonItems.Length > idx)
                {
                    var barItem = rightButtonItems[idx];
                    if (barItem != null)
                    {
                        barItem.UpdateBadge(value, backgroundColor.ToUIColor(), textColor.ToUIColor());
                    }
                }

            });
        }
    }
}

using System;
using Android.Content;
using Com.OneSignal.Android;

namespace Covid19.Droid
{
    public class NotificationExtender : NotificationExtenderService
    {
        protected override void OnHandleIntent(Intent intent)
        {
            throw new NotImplementedException();
        }

        protected override bool OnNotificationProcessing(OSNotificationReceivedResult receivedResult)
        {
            return false;
        }
    }
}
           
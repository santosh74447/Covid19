using System;
using Xamarin.Forms;

namespace Covid19.Models
{
    public class NotificationPageModel
    {
        //public string EmailId { get; set; }
        //public string Pwd { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string IsPromotionActive { get; set; }
        //public string DeviceId { get; set; }
        //public string CountryName { get; set; }
        public int NotificationId { get; set; }
        public string NDescription { get; set; }
        public string NCreatedDate { get; set; }
        public string Country { get; set; }
        public string Flag { get; set; }
        public string Action { get; set; }
        public string newNotificationCount { get; set; }
        
        public FontAttributes IsReadType { get; set; }

        private string _isread;
        public string IsRead
        {
            get => _isread;

            set
            {
                _isread = value;

                if (_isread.Equals("TRUE"))
                {
                    IsReadType = FontAttributes.None;
                }
                else
                {
                    IsReadType = FontAttributes.Bold;
                }
            }
        }
    }
}

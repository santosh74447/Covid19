using System;
using Xamarin.Forms;

namespace Covid19.Services
{
    public interface IBadgeService
    {
        void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor);

    }
}

using Clone.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Diagnostics.Contracts;

namespace Clone.Web.Components.Pages
{
    public partial class AdminUser
    {
        const string MESSAGEBAR_SECTION = "MESSAGEBAR_SERVICE_DEFAULT";

        async Task ShowMessageBar()
        {
            var result = await NotificationService.ShowMessageBarAsync(options =>
            {
                options.Section = MESSAGEBAR_SECTION;

                options.Intent = MessageBarIntent.Success;
                options.Layout = MessageBarLayout.Notification;
                options.Title = "Delete operation";
                options.Message = "Successfully deleted 'XYZ-blazor.pdf'";
                options.AllowDismiss = true;
                options.Lifetime = TimeSpan.FromSeconds(5);
                options.ResultTiming = MessageBarResultTiming.Closed;

                options.OnStatusChange = (args) =>
                {
                    Console.WriteLine($"Message bar status changed: {args.Status}");
                };
            });

            Console.WriteLine($"Message bar closed: {result.Reason}");
        }
    }
}

using Clone.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;

namespace Clone.Web.Components.Account
{
    // Remove the "else if (EmailSender is IdentityNoOpEmailSender)" block from RegisterConfirmation.razor after updating with a real implementation.
    internal sealed class IdentityNoOpEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IEmailSender emailSender = new NoOpEmailSender();
        private readonly IStringLocalizer<SharedResource> localizer;

        public IdentityNoOpEmailSender(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            emailSender.SendEmailAsync(
                email,
                localizer["EmailSender.ConfirmSubject"],
                localizer["EmailSender.ConfirmBody", confirmationLink]);

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            emailSender.SendEmailAsync(
                email,
                localizer["EmailSender.ResetPasswordSubject"],
                localizer["EmailSender.ResetPasswordBody", resetLink]);

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            emailSender.SendEmailAsync(
                email,
                localizer["EmailSender.ResetPasswordSubject"],
                localizer["EmailSender.ResetPasswordCodeBody", resetCode]);
    }
}

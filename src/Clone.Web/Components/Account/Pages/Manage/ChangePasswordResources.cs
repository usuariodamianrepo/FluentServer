using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="ChangePassword"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class ChangePasswordResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.ChangePassword",
            typeof(ChangePasswordResources).Assembly);

        public static string CurrentPassword => GetString("CurrentPassword");

        public static string NewPassword => GetString("NewPassword");

        public static string ConfirmNewPassword => GetString("ConfirmNewPassword");

        public static string PasswordLength => GetString("Validation.PasswordLength");

        public static string PasswordMismatch => GetString("Validation.PasswordMismatch");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

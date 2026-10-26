using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="SetPassword"/> page, so they can be used from the DataAnnotations
    /// attributes of the form model.
    /// </summary>
    public static class SetPasswordResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.SetPassword",
            typeof(SetPasswordResources).Assembly);

        public static string NewPassword => GetString("Label.NewPassword");

        public static string ConfirmPassword => GetString("Label.ConfirmPassword");

        public static string Required => GetString("Validation.Required");

        public static string PasswordLength => GetString("Validation.PasswordLength");

        public static string PasswordMismatch => GetString("Validation.PasswordMismatch");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

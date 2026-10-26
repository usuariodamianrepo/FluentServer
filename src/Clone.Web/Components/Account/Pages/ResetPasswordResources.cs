using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="ResetPassword"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class ResetPasswordResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.ResetPassword",
            typeof(ResetPasswordResources).Assembly);

        public static string Email => GetString("Email");

        public static string Password => GetString("Password");

        public static string ConfirmPassword => GetString("ConfirmPassword");

        public static string Required => GetString("Validation.Required");

        public static string EmailValidation => GetString("Validation.Email");

        public static string StringLength => GetString("Validation.StringLength");

        public static string PasswordMismatch => GetString("Validation.PasswordMismatch");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

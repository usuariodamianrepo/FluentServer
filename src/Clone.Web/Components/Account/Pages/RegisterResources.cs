using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="Register"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class RegisterResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Register",
            typeof(RegisterResources).Assembly);

        public static string Email => GetString("Email");

        public static string Name => GetString("Name");

        public static string PhoneNumber => GetString("PhoneNumber");

        public static string Password => GetString("Password");

        public static string ConfirmPassword => GetString("ConfirmPassword");

        public static string StreetAddress => GetString("StreetAddress");

        public static string City => GetString("City");

        public static string State => GetString("State");

        public static string PostalCode => GetString("PostalCode");

        public static string Required => GetString("Validation.Required");

        public static string EmailValidation => GetString("Validation.Email");

        public static string StringLength => GetString("Validation.StringLength");

        public static string PasswordMismatch => GetString("Validation.PasswordMismatch");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

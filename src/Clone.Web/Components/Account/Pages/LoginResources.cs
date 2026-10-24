using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="Login"/> page, so they can be used from the DataAnnotations
    /// attributes of the form model.
    /// </summary>
    public static class LoginResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Login",
            typeof(LoginResources).Assembly);

        public static string Email => GetString("Email");

        public static string Password => GetString("Password");

        public static string RememberMe => GetString("RememberMe");

        public static string Required => GetString("Validation.Required");

        public static string EmailValidation => GetString("Validation.Email");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

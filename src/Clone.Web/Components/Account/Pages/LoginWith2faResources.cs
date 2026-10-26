using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="LoginWith2fa"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class LoginWith2faResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.LoginWith2fa",
            typeof(LoginWith2faResources).Assembly);

        public static string AuthenticatorCode => GetString("Label.AuthenticatorCode");

        public static string RememberMachine => GetString("Label.RememberMachine");

        public static string Required => GetString("Validation.Required");

        public static string StringLength => GetString("Validation.StringLength");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

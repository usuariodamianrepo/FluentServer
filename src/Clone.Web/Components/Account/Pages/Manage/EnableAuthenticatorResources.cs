using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="EnableAuthenticator"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class EnableAuthenticatorResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.EnableAuthenticator",
            typeof(EnableAuthenticatorResources).Assembly);

        public static string VerificationCode => GetString("VerificationCode");

        public static string Required => GetString("Validation.Required");

        public static string CodeLength => GetString("Validation.CodeLength");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

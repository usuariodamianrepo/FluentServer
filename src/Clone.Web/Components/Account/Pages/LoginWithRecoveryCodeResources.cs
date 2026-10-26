using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="LoginWithRecoveryCode"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class LoginWithRecoveryCodeResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.LoginWithRecoveryCode",
            typeof(LoginWithRecoveryCodeResources).Assembly);

        public static string RecoveryCode => GetString("Label.RecoveryCode");

        public static string Required => GetString("Validation.Required");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

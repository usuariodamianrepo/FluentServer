using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="ResendEmailConfirmation"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class ResendEmailConfirmationResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.ResendEmailConfirmation",
            typeof(ResendEmailConfirmationResources).Assembly);

        public static string Email => GetString("Email");

        public static string Required => GetString("Validation.Required");

        public static string EmailValidation => GetString("Validation.Email");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

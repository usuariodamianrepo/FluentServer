using System.Resources;

namespace Clone.Web.Components.Account.Pages
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="ExternalLogin"/> page, so they can be used from the
    /// DataAnnotations attributes of the form model.
    /// </summary>
    public static class ExternalLoginResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.ExternalLogin",
            typeof(ExternalLoginResources).Assembly);

        public static string EmailRequired => GetString("Validation.Required");

        public static string InvalidEmail => GetString("Validation.InvalidEmail");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="Email"/> page, so they can be used from the DataAnnotations
    /// attributes of the form model.
    /// </summary>
    public static class EmailResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.Email",
            typeof(EmailResources).Assembly);

        public static string Email => GetString("Email");

        public static string NewEmail => GetString("NewEmail");

        public static string EmailRequired => GetString("Validation.Required");

        public static string InvalidEmail => GetString("Validation.InvalidEmail");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

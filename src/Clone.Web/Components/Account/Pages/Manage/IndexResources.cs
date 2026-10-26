using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="Index"/> page, so they can be used from the DataAnnotations
    /// attributes of the form model.
    /// </summary>
    public static class IndexResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.Index",
            typeof(IndexResources).Assembly);

        public static string PhoneNumber => GetString("PhoneNumber");

        public static string InvalidPhone => GetString("Validation.Phone");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

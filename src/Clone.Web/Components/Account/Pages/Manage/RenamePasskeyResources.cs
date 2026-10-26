using System.Resources;

namespace Clone.Web.Components.Account.Pages.Manage
{
    /// <summary>
    /// Provides strongly typed access to the localized resources of the
    /// <see cref="RenamePasskey"/> page, so they can be used from the DataAnnotations
    /// attributes of the form model.
    /// </summary>
    public static class RenamePasskeyResources
    {
        private static readonly ResourceManager ResourceManager = new(
            "Clone.Web.Resources.Components.Account.Pages.Manage.RenamePasskey",
            typeof(RenamePasskeyResources).Assembly);

        public static string PasskeyNameLength => GetString("Validation.PasskeyNameLength");

        private static string GetString(string name) => ResourceManager.GetString(name) ?? string.Empty;
    }
}

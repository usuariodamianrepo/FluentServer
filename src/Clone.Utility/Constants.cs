namespace Clone.Utility
{
    public static class MaxRows
    {
        public const int MaxThirty = 30;
        public const int MaxFifty = 50;
        public const int MaxOneHundred = 100;
    }

    /// <summary>
    /// IANA funciona nativo en .NET Core en Linux/Docker
    /// </summary>
    public static class Iana
    {
        public const string MyZone = "America/Argentina/Buenos_Aires";
    }

    /// <summary>
    /// Role Definitions (RD) constants for roles in Spanish.
    /// </summary>
    public static class RD
    {
        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Cliente";
        public const string RoleSupplier = "Proveedor";
        public const string RoleEmployee = "Empleado";
    }

    public static class Constants
    {
        public const string TabList = "tab-list";
        public const string TabContent = "tab-content";
        public const string TabDetails = "tab-details";
        public const string OptionEmpty = "-- Empty --";
        public const int ItemsPerPage = 10;
        public const int ItemsMaxNumber = 100;

        public const string KeyEnter = "Enter";
        public const int KeyCodeEnter = 13;
    }
}

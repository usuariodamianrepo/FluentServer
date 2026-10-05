namespace Clone.Utility
{
    public static class MaxRows
    {
        public const int MaxFifty = 50;
        public const int MaxOneHundred = 100;
    }

    /// <summary>
    /// IANA funciona nativo en .NET Core en Linux/Docker
    /// </summary>
    public static class IANA
    {
        public const string ZonaArgentina = "America/Argentina/Buenos_Aires";
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

    /// <summary>
    /// Screen Language (SL) constants for Spanish localization.
    /// </summary>
    public static class SL
    {
        public const string MsgConcurrencyError = "Error de concurrencia. Vuelva a la lista e inténtelo de nuevo.";
        public const string MsgDeletedSuccessfully = "Se ha eliminado correctamente.";
        public const string MsgErrorDelete = "NO se puede eliminar.";
        public const string MsgErrorIsRelated = "Está relacionado a otro registro.";
        public const string MsgErrorNotExists = "NO existe.";
        public const string MsgInvalidOperation = "Operación NO válida.";
        public const string MsgModelErrors = "Errores: ";
        public const string MsgSavedSuccessfully = "Guardado!";
        public const string MsgSearch50 = "Tu búsqueda ha superado los 50 registros, necesitas mejorarla.";

        public const string BtnCreate = "Crear";
        public const string BtnDelete = "Eliminar";
        public const string BtnEdit = "Editar";
        public const string BtnDetails = "Detalles";
        public const string BtnSave = "Guardar";
        public const string BtnBackToList = "Volver a la lista";
        public const string BtnCancel = "Cancelar";
        public const string BtnYes = "Sí";
        public const string BtnNo = "No";
        public const string BtnClose = "Cerrar";
        public const string BtnUpload = "Subir";
        public const string BtnDownload = "Descargar";
        public const string BtnSearch = "Buscar";
        public const string BtnClear = "Limpiar";
        public const string BtnApplyFilter = "Aplicar filtro";
        public const string BtnRemoveFilter = "Quitar filtro";
        public const string BtnGenerateReport = "Generar informe";
        public const string BtnExportToCSV = "Exportar a CSV";
        public const string BtnExportToExcel = "Exportar a Excel";
        public const string BtnExportToPdf = "Exportar a PDF";
        public const string BtnSendEmail = "Enviar correo";
        public const string BtnPrint = "Imprimir";
        public const string BtnPermits = "Permisos";
    }
}

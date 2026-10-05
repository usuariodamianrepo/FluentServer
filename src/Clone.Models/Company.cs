using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public sealed class Company : AuditableEntity
    {
		[DisplayName("Nombre")]
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[MaxLength(120)]
		public string Name { get; set; } = string.Empty;

        [DisplayName("Dirección")]
		[MaxLength(120)]
		public string? StreetAddress { get; set; }

        [DisplayName("Ciudad")]
		[MaxLength(120)]
		public string? City { get; set; }

        [DisplayName("Provincia")]
		[MaxLength(60)]
		public string? State { get; set; }

        [DisplayName("Código Postal")]
		[MaxLength(60)]
		public string? PostalCode { get; set; }

        [DisplayName("Teléfono")]
		[MaxLength(60)]
		public string? PhoneNumber { get; set; }
    }
}

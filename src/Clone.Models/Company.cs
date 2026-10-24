using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public sealed class Company : AuditableEntity
    {
		[Required]
		[MaxLength(120)]
		public string Name { get; set; } = string.Empty;

		[MaxLength(120)]
		public string? StreetAddress { get; set; }

		[MaxLength(120)]
		public string? City { get; set; }

		[MaxLength(60)]
		public string? State { get; set; }

		[MaxLength(60)]
		public string? PostalCode { get; set; }

		[MaxLength(60)]
		public string? PhoneNumber { get; set; }
    }
}

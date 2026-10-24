using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public sealed class ApplicationUser : IdentityUser
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

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}

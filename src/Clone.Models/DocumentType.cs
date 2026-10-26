using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public class DocumentType : AuditableEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(450)]
        public string Description { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public class Document : AuditableEntity
    {
        [Required]
        [MaxLength(120)]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "timestamp without time zone")]
        [Required]
        public DateTime Published { get; set; }

        [Required]
        [MaxLength(450)]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; } = false;

        [Required]
        [MaxLength(250)]
        public string URL { get; set; } = string.Empty;

        [ForeignKey("DocumentTypeId")]
        [Required]
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; } = default!;
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public sealed class Document : AuditableEntity
    {
        [MaxLength(120)]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime Published { get; set; }

        [MaxLength(450)]
        public string? Description { get; set; }

        public bool IsPublic { get; set; } = false;

        [MaxLength(250)]
        public string? URL { get; set; }

        [ForeignKey("DocumentTypeId")]
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; } = default!;
    }
}
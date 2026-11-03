using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public class DocumentType : AuditableEntity
    {
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(450)]
        public string? Description { get; set; }
    }
}
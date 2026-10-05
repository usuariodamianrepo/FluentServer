using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public class Document : AuditableEntity
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(120)]
        public string Title { get; set; } = string.Empty;

        [DisplayName("Publicado")]
        [Column(TypeName = "timestamp without time zone")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime Published { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(450)]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Público")]
        public bool IsPublic { get; set; } = false;

        [DisplayName("URL")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(250)]
        public string URL { get; set; } = string.Empty;

        [DisplayName("Tipo de Documento")]
        [ForeignKey("DocumentTypeId")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; } = default!;
    }
}
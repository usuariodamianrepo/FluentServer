using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public class DocumentType : AuditableEntity
    {
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(450)]
        public string Description { get; set; } = string.Empty;
    }
}
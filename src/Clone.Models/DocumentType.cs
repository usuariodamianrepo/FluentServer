using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public class DocumentType : AuditableEntity
    {
<<<<<<< HEAD
        [Required(ErrorMessage = "The {0} field is required.")]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The {0} field is required.")]
=======
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
>>>>>>> dev
        [MaxLength(450)]
        public string Description { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;

namespace Clone.Models
{
    public sealed class CostCenter : AuditableEntity
    {
		[MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

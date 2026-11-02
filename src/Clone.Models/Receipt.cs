using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public sealed class Receipt : AuditableEntity
    {
        [Column(TypeName = "timestamp without time zone")]
        public DateTime Date { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } = default!;

        [Required]
        public int CostCenterId { get; set; }
        [ForeignKey("CostCenterId")]
        [ValidateNever]
        public CostCenter CostCenter { get; set; } = default!;

        [MaxLength(60)]
        public string? Number { get; set; }

        [MaxLength(180)]
        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int? OrderId { get; set; }

        public bool Active { get; set; } = true;

        [NotMapped]
        public bool IsIngress => Amount >= 0;

        [NotMapped]
        public bool CanUpdate => Active && (OrderId is null);

        public void SetAmount(bool isIngress)
        {
            decimal absAmount = Math.Abs(Amount);
            Amount = isIngress ? absAmount : -absAmount;
        }
    }
}

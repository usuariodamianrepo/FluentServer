param(
	[Parameter(Mandatory = $true, HelpMessage = "Provide the Entity Name.")]
    [ValidateNotNullOrEmpty()]
	[string]$EntityName
)

$entityContent = @"
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clone.Models
{
    public sealed class $EntityName : AuditableEntity
    {
        [Display(Name = "Nombre"), MaxLength(60)]
        public string Name { get; set; } = string.Empty;
        
	    [Display(Name = "Descripción"), MaxLength(450)]
        public string? Description { get; set; }
        
        [Display(Name = "Precio"), Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Price { get; set; }

	    [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Relationship")]
        public int RelationshipId { get; set; }
        [ForeignKey("RelationshipId")]
        [ValidateNever]
        [Display(Name = "Relationship")]
        public Relationship Relationship { get; set; } = default!;
    }
}
"@

New-Item -Path "../src/Clone.Models/$EntityName.cs" -ItemType File -Force
Set-Content -Path "../src/Clone.Models/$EntityName.cs" -Value $entityContent

$entityRepository = $iEntityServiceName = -join($EntityName + "Repository")
$entityIRepository = $iEntityServiceName = -join("I" + $EntityName + "Repository")
$repContent = @"
using Clone.DataAccess.Repositories.IRepository;
using Clone.Models;

namespace Clone.DataAccess.Repositories
{
    public class $entityRepository : $entityIRepository
    {
        private readonly IRepositoryAsync<$EntityName> _repository;

        public $entityRepository(IRepositoryAsync<$EntityName> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
"@

New-Item -Path "../src/Clone.DataAccess/Repositories/$entityRepository.cs" -ItemType File -Force
Set-Content -Path "../src/Clone.DataAccess/Repositories/$entityRepository.cs" -Value $repContent

$repIContent = @"
namespace Clone.DataAccess.Repositories.IRepository
{
	public interface $entityIRepository 
	{

	}
}
"@

New-Item -Path "../src/Clone.DataAccess/Repositories/IRepository/$entityIRepository.cs" -ItemType File -Force
Set-Content -Path "../src/Clone.DataAccess/Repositories/IRepository/$entityIRepository.cs" -Value $repIContent

Write-Host "Congratulations! the $EntityName entity was created successfully"
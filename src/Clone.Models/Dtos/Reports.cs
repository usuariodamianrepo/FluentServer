using System.ComponentModel;

namespace Clone.Models.Dtos
{
    public record BalancesDto(string Id, int? CompanyId, string? CompanyName, string Name, string? PhoneNumber, decimal Total);
    public record CostCenterTotalDto(int CostCenterId, string? CostCenterName, int TotalCount, decimal SubTotal);
}

namespace Clone.Models.Dtos
{
    public record DebsDto(string Id, int? CompanyId, string? CompanyName, string Name, string? PhoneNumber, decimal Total);
}

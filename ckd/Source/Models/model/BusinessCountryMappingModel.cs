
using FluentValidation.Results;
using Models.common;
namespace Models.model;

public class BusinessCountryMappingModel : BaseModel
{
    public int? Id { get; set; }
    public int? BusinessId { get; set; }
    public int? CountryId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public string? CountryName { get; set; }
    public string? BusinessName { get; set; }
    public string? CountryCode { get; set; }
    public int? SortNo { get; set; }

}

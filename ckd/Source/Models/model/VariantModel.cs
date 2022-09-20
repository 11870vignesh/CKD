using System.Text.Json.Serialization;
using Models.entity;
using Models.model;
namespace Models.model;


public class VariantModel : BaseModel
{
    public int? Id { get; set; }
    public string? VariantName { get; set; }
    public int? CountryId { get; set; }
    public int? BusinessId { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public List<BusinessCountryMappingModel>? CountryList { get; set; }
    public List<BusinessModel>? BusinessList { get; set; }

    ///public List<ModuleModel>? ModuleModel { get; set; }
    //public List<ValidationFailure>? Errors { get; set; }


}
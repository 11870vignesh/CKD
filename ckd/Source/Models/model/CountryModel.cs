namespace Models.model;

public class CountryModel : BaseModel
{
    public int? Id { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public int? BusinessMappingId { get; set; }
    public int? BusinessId { get; set; }
    public string? BusinessName { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public List<BusinessModel>? BusinessList { get; set; }
}
// public class CountryCreateModel
// {
//     public CountryCreateModel()
//     {
//         CountryData = new CountryModel();
//     }
//     public CountryModel? CountryData { get; set; }


// }
namespace Models.entity;

using System.Text.Json.Serialization;
public class Country
{
    public int? Id { get; set; }
    public string? CountryName { get; set; }
    public string? CountryCode { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public virtual List<BusinessCountryMapping> BusinessCountryMapping { get; set; }

}
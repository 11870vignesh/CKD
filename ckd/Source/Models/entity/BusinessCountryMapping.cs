namespace Models.entity;

using System.Text.Json.Serialization;
public class BusinessCountryMapping
{
    public int? Id { get; set; }
    public int? BusinessId { get; set; }
    public int? CountryId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public int? SortNo { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public virtual Country Country { get; set; }
    public virtual Business Business { get; set; }
}
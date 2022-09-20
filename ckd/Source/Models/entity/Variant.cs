using System;
namespace Models.entity;
public class Variant : BaseEntity
{
    public int? Id { get; set; }
    public string? VariantName { get; set; }
    public int? CountryId { get; set; }
    public int? BusinessId { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    //public virtual Module Module { get; set; }

}
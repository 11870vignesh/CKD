using System;
namespace Models.entity;
public class Module : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int ModuleId { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
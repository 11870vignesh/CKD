
using FluentValidation.Results;
using Models.common;
namespace Models.model;

public class BusinessModel : BaseModel
{
    // public BusinessModel()
    // {
    //     validationMessage = new List<ValidationMessageModel>();
    // }
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? SortNo { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDelete { get; set; }
    public int? Flag { get; set; }

}

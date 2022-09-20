using FluentValidation.Results;
namespace Models.model;

using System;
using Models.common;

public abstract class BaseModel
{
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }

    public string? CreatedByUserName { get; set; }
    public string? ModifiedByUserName { get; set; }

    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public Session? thisSession { get; set; }

    public string? UserMessage { get; set; }
    public List<string>? UserMessages { get; set; }
    public int? StatusCode { get; set; }
    public List<ValidationFailure>? ValidationErrors { get; set; }
    public int? Total { get; set; }

}



namespace Models.common;

using FluentValidation.Results;
using Models.model;

public class WebApiResponse<T> where T : BaseModel
{
    public int HttpStatusCode { get; set; }
    public string? HttpStatusMessage { get; set; }

    public T? Data { get; set; }
    public List<T>? DataList { get; set; }
    public List<ValidationFailure>? ValidationErrors { get; set; }

}
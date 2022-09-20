using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Models.entity;
using Models.model;

namespace Web.Controllers;
public class AccessController : BaseController<AccessController>
{

    private readonly IConfiguration _config;

    public AccessController(ILogger<AccessController> logger, IConfiguration config) : base(logger, config)
    {
        _config = config;
    }

    protected void CheckAndAddModelStateError(List<ValidationFailure> ValidationErrors)
    {
        if (ValidationErrors.Any())
        {
            foreach (var item in ValidationErrors)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
        }

    }
    public async Task<IActionResult> GetAccessDataForDataTable()
    {
        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        // Skiping number of Rows count  
        var start = Request.Form["start"].FirstOrDefault();
        //var start = data.start;
        // Paging Length 10,20  
        var length = Request.Form["length"].FirstOrDefault();
        // Sort Column Name  
        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        // Sort Column Direction ( asc ,desc)  
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        // Search Value from (Search box)  
        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        int skip = start != null ? Convert.ToInt32(start) : 0;
        var accessName = Request.Form["columns[0][search][value]"].FirstOrDefault();
        var accessDate = Request.Form["columns[1][search][value]"].FirstOrDefault();

        if (searchValue == null || searchValue == "undefined" || searchValue == "")

            searchValue = "null";

        if (accessDate == null || accessDate == "undefined" || accessDate == "")

            accessDate = "null";

        if (accessName == null || accessName == "undefined" || accessName == "")

            accessName = "null";

        if (length == null || length == "undefined" || length == "")

            length = "null";


        if (sortColumn == null || sortColumn == "undefined" || sortColumn == "")

            sortColumn = "null";

        if (sortColumnDirection == null || sortColumnDirection == "undefined" || sortColumnDirection == "")

            sortColumnDirection = "null";

        string urlJsonKey = "AccessTable";
        string queryParameter = "?search=" + searchValue + "&limit=" + pageSize + "&offset=" + skip + "&order=" + sortColumn + "&dir=" + sortColumnDirection + "&business=" + accessName + "&searchdate=" + accessDate;
        var AccessData = await PostRequest<AccessModel, WebApiResponse<AccessModel>>(urlJsonKey, queryParameter, null);
        return Json(new { draw = draw, recordsFiltered = AccessData.DataList[0].Total, recordsTotal = AccessData.DataList[0].Total, data = AccessData.DataList });
    }
        public async Task<IActionResult> Index(int? Id)
    {
        var modelData = new AccessModel();

        if (Id != null && Id != 0)
        {
            string urlJsonKey = "GetByIDAccessURL";
            string queryParameter = "?Id=" + Id;
            var apiResp = await GetRequest<WebApiResponse<AccessModel>>(urlJsonKey, queryParameter);
            if (apiResp.HttpStatusCode == 200)
                modelData = apiResp.Data;
        }

        return View(modelData);

    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Index(AccessModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        string urlJsonKey = "";
        if (model.Id != null && model.Id != 0)
        {
            urlJsonKey = "UpdateAccessURL";
            model.ModifiedOn = DateTime.Now;
        }
        else
        {
            urlJsonKey = "CreateAccessURL";
            model.CreatedOn = DateTime.Now;
        }
        var apiResponse = await PostRequest<AccessModel, WebApiResponse<AccessModel>>(urlJsonKey, "", model);

        if (apiResponse.HttpStatusCode == 200)
        {
            ViewData.ModelState.Clear();
            model = new AccessModel();
            model.StatusCode = 200;
        }
        else
            CheckAndAddModelStateError(apiResponse.ValidationErrors);

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(int entityId)
    {
        var urlJsonKey = "DeleteAccessURL";
        string queryParameter = "?entityId=" + entityId;
        //Get  mthod
        var apiResp = await GetRequest<WebApiResponse<AccessModel>>(urlJsonKey, queryParameter);

        // var apiResponse = await PostRequest<AccessModel, WebApiResponse<AccessModel>>(urlJsonKey, queryParameter, null);
        return Json(apiResp);
    }
}
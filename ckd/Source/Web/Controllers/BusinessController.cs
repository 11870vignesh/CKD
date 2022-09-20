using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Models.entity;
using Models.model;
using Models.common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Web.Controllers;
public class BusinessController : BaseController<BusinessController>
{

    public BusinessController(ILogger<BusinessController> logger, IConfiguration config) : base(logger, config)
    {

    }

    [HttpGet]
    public IActionResult Index()
    {
        BusinessModel business = new BusinessModel();
        return View(business);
    }
    [HttpPost]
    public async Task<IActionResult> Index(BusinessModel business)
    {
        var relativeUrl = "";
        if (business.Id == null)
        {
            relativeUrl = "CreateBusiness";
            business.CreatedOn = DateTime.Now;
            business.IsActive = true;
            business.IsDelete = false;
        }
        else
        {
            relativeUrl = "UpdateBusiness";
            business.ModifiedOn = DateTime.Now;
        }

        if (ModelState.IsValid)
        {
            var apiResponse = await PostRequest<BusinessModel, WebApiResponse<BusinessModel>>(relativeUrl, "", business);
            if (apiResponse.HttpStatusCode == 200)
            {
                BusinessModel Data = new BusinessModel();
                Data.StatusCode = 200;
                ViewData.ModelState.Clear();
                return View(Data);
            }
            else
            {
                if (apiResponse.HttpStatusCode == 400)
                {

                    if (apiResponse.Data.ValidationErrors != null)
                    {
                        CheckAndAddModelStateError(apiResponse.Data.ValidationErrors);
                    }
                    return View(apiResponse.Data);
                }
                else
                {
                    business.StatusCode = 500;
                    return View(business);
                }
            }
        }
        else
        {
            return View(business);
        }




    }
    public async Task<IActionResult> BusinessData()
    {

        // var Results = JsonConvert.DeserializeObject<JsonToken>(a);
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
        var businessName = Request.Form["columns[0][search][value]"].FirstOrDefault();
        var businessDate = Request.Form["columns[1][search][value]"].FirstOrDefault();
        //var country = Request.Form["columns[2][search][value]"].FirstOrDefault();

        if (searchValue == null || searchValue == "undefined" || searchValue == "")
            searchValue = "null";

        if (businessDate == null || businessDate == "undefined" || businessDate == "")
            businessDate = "null";

        if (businessName == null || businessName == "undefined" || businessName == "")
            businessName = "null";

        if (length == null || length == "undefined" || length == "")
            length = "null";

        if (sortColumn == null || sortColumn == "undefined" || sortColumn == "")
            sortColumn = "null";

        if (sortColumnDirection == null || sortColumnDirection == "undefined" || sortColumnDirection == "")
            sortColumnDirection = "null";

        var urlJsonKey = "GetBusinessData";
        string queryParameter = "?search=" + searchValue + "&limit=" + pageSize + "&offset=" + skip + "&order=" + sortColumn + "&dir=" + sortColumnDirection + "&business=" + businessName + "&searchdate=" + businessDate;


        var BusinessData = await PostRequest<BusinessModel, WebApiResponse<BusinessModel>>(urlJsonKey, queryParameter, null);
        if (BusinessData.DataList.Count() == 0)
        {
            return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = BusinessData.DataList });
        }
        else
        {
            return Json(new { draw = draw, recordsFiltered = BusinessData.DataList[0].Total, recordsTotal = BusinessData.DataList[0].Total, data = BusinessData.DataList });
        }
    }
    public async Task<IActionResult> GetById(int? Id)
    {
        var relativeUrl = "GetBusinessById";
        string queryParameter = "?Id=" + Id;
        var BusinessDataResult = await PostRequest<BusinessModel, WebApiResponse<BusinessModel>>(relativeUrl, queryParameter, null);
        return View("Index", BusinessDataResult.Data);
    }

    public async Task<IActionResult> DeleteBusiness(BusinessModel business)
    {
        var relativeUrl = "DeleteBusiness";
        var BusinessResultData = await PostRequest<BusinessModel, WebApiResponse<BusinessModel>>(relativeUrl, "", business);
        if (BusinessResultData.HttpStatusCode == 200)
        {
            BusinessModel Data = new BusinessModel();
            Data.StatusCode = 200;
            ViewData.ModelState.Clear();
            logger.LogInformation("business details deleted sucessfully");
            return View("Index", Data);
        }
        else
        {
            if (BusinessResultData.HttpStatusCode == 400)
            {
                business.StatusCode = 400;
                //CheckAndAddModelStateError(BusinessResultData.Data.ValidationErrors);
                business.UserMessage = BusinessResultData.Data.ValidationErrors[0].ErrorMessage;
                return View("Index", BusinessResultData.Data);
            }
            else
            {
                logger.LogInformation("something went wrong");
                business.StatusCode = 500;
                return View("Index", business);
            }
        }
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


}

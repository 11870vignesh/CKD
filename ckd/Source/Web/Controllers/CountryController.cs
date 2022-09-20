using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Models.entity;
using Models.model;
using Models.common;

namespace Web.Controllers;

//[Authorize]
public class CountryController : BaseController<CountryController>
{
    private object mapper;

    public CountryController(ILogger<CountryController> logger, IConfiguration config) : base(logger, config)
    {

    }
    private List<BusinessModel> GetAllBusiness()
    {
        var relativeUrl = "business/GetAllBusiness";
        var BusinessDataResult = GetRequest<WebApiResponse<BusinessModel>>(relativeUrl, "");
        return BusinessDataResult.Result.DataList;
    }
    [HttpGet]
    public async Task<ActionResult> Index()
    {
        var countryModel = new CountryModel();
        countryModel.BusinessList = GetAllBusiness();
        return View(countryModel);
    }
    [HttpPost]
    public async Task<ActionResult> Index(CountryModel Country)
    {
        var relativeUrl = "country/Update";
        if (Country.BusinessMappingId == null || Country.BusinessMappingId == 0)
            relativeUrl = "country/Create";

        var CountryInsertResponse = await PostRequest<CountryModel, WebApiResponse<CountryModel>>(relativeUrl, "", Country);
        Country.StatusCode = CountryInsertResponse.HttpStatusCode;
        if (CountryInsertResponse.HttpStatusCode == 200)
        {
            var countryModel = new CountryModel();
            logger.LogInformation("business details has been saved.");
            countryModel.BusinessList = GetAllBusiness();
            return View("Index", countryModel);
        }
        else
        {
            var countryModel = new CountryModel();
            countryModel.CountryName = Country.CountryName;
            countryModel.CountryCode = Country.CountryCode;
            countryModel.BusinessId = Country.BusinessId;
            countryModel.BusinessList = GetAllBusiness();
            if (CountryInsertResponse.Data.ValidationErrors != null)
            {
                CheckAndAddModelStateError(CountryInsertResponse.Data.ValidationErrors);
                countryModel.ValidationErrors = CountryInsertResponse.Data.ValidationErrors;
            }
            return View(countryModel);
        }
    }
    public async Task<IActionResult> LoadFilter()
    {
        var relativeUrlForBusiness = "business/getall";
        var BusinessDataResult = await GetRequest<WebApiResponse<BusinessModel>>(relativeUrlForBusiness, "");
        var relativeUrlForCountry = "country/getall";
        var CountryDataResult = await GetRequest<WebApiResponse<CountryModel>>(relativeUrlForCountry, "");
        return Json(new { business_list = BusinessDataResult.DataList, country_list = CountryDataResult.DataList });
    }
    public async Task<IActionResult> LoadCountry(BusinessCountryMappingModel data)
    {
        var relativeUrlForCountry = "country/GetCountryByBusiness";
        var CountryData = await PostRequest<BusinessCountryMappingModel, WebApiResponse<BusinessCountryMappingModel>>(relativeUrlForCountry, "", data);
        return Json(new { CountryList = CountryData.DataList });
    }

    public async Task<IActionResult> CountryData()
    {
        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        // Skiping number of Rows count  
        var start = Request.Form["start"].FirstOrDefault();
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
        var business = Request.Form["columns[1][search][value]"].FirstOrDefault();
        var country = Request.Form["columns[2][search][value]"].FirstOrDefault();
        var code = Request.Form["columns[3][search][value]"].FirstOrDefault();

        if (searchValue == null || searchValue == "undefined" || searchValue == "")
        {
            searchValue = "null";
        }
        if (length == null || length == "undefined" || length == "")
        {
            length = "null";
        }
        if (start == null || start == "undefined" || start == "")
        {
            start = "null";
        }
        if (sortColumn == null || sortColumn == "undefined" || sortColumn == "")
        {
            sortColumn = "null";
        }
        if (sortColumnDirection == null || sortColumnDirection == "undefined" || sortColumnDirection == "")
        {
            sortColumnDirection = "null";
        }
        if (code == null || code == "undefined" || code == "")
            code = "null";
        if (country == null || country == "undefined" || country == "")
            country = "null";
        if (business == null || business == "undefined" || business == "")
            business = "null";
        // + "&business=" + business;

        var relativeUrl = "country/getallcountrydata?" + "search=" + searchValue + "&limit=" + pageSize + "&offset=" + skip + "&order=" + sortColumn + "&dir=" + sortColumnDirection + "&business=" + business + "&country=" + country + "&code=" + code;
        var CountryData = await PostRequest<BusinessCountryMappingModel, WebApiResponse<BusinessCountryMappingModel>>(relativeUrl, "", null);
        if (CountryData.DataList.Count() == 0)
        {
            return Json(new { draw = draw, recordsFiltered = 0, recordsTotal = 0, data = CountryData.DataList });
        }
        else
        {
            return Json(new { draw = draw, recordsFiltered = CountryData.DataList.Count(), recordsTotal = CountryData.DataList.Count(), data = CountryData.DataList });
        }

    }
    public async Task<IActionResult> GetCountry(string Id)
    {
        var relativeUrl = "Country/getbyid";
        var id = Convert.ToInt16(Id);
        CountryModel CountryGetData = new CountryModel();
        CountryGetData.Id = id;
        var CountryGetByIdResult = await PostRequest<CountryModel, WebApiResponse<CountryModel>>(relativeUrl, "", CountryGetData);
        var relativeUrlForBusiness = "business/getall";
        var BusinessDataResult = await GetRequest<List<BusinessModel>>(relativeUrlForBusiness, "");
        //CountryGetByIdResult.Data. = BusinessDataResult;
        return Json(CountryGetByIdResult.Data);
    }
    public async Task<IActionResult> GetById(CountryModel country)
    {
        var relativeUrl = "Country/getbyid";
        var CountryDataResult = await PostRequest<CountryModel, WebApiResponse<CountryModel>>(relativeUrl, "", country);
        CountryDataResult.Data.BusinessList = GetAllBusiness();
        return View("Index", CountryDataResult.Data);
    }
    public async Task<IActionResult> DeleteCountry(CountryModel country)
    {
        var relativeUrl = "country/delete";
        var DeleteCountryResult = await PostRequest<CountryModel, WebApiResponse<CountryModel>>(relativeUrl, "", country);

        if (DeleteCountryResult.HttpStatusCode == 200)
        {
            var countryModel = new CountryModel();
            countryModel.BusinessList = GetAllBusiness();
            logger.LogInformation("country detailss has been deleted. ");
            return View("Index", countryModel);
        }
        else
        {

            logger.LogInformation("Invalid Details");
            return View();
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

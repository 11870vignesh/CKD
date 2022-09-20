using Microsoft.AspNetCore.Authorization;
using FluentValidation.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Models.entity;
using WebAPI.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using Models.model;
using WebAPI.services;
using Models.common;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class CountryController : BaseApiController<CountryController>
{
    private ICountryService countryService;

    public CountryController(ILogger<CountryController> logger, IMapper mapper, ICountryService countrySvc) : base(logger, mapper)
    {
        countryService = countrySvc;
    }
    [HttpPost("Create")]
    public WebApiResponse<CountryModel> Create(CountryModel country)
    {
        return CreateOrUpdate(country);
    }

    [HttpPost("Update")]
    public WebApiResponse<CountryModel> Update(CountryModel country)
    {
        return CreateOrUpdate(country);
    }

    private WebApiResponse<CountryModel> CreateOrUpdate(CountryModel country)
    {
        var apiResp = new WebApiResponse<CountryModel>();
        try
        {
            CountryValidator BusinessValidation = new CountryValidator(countryService);
            var ValidationResult = BusinessValidation.Validate(country);
            if (ValidationResult.IsValid)
            {
                var result = new CountryModel();
                if (country.Id == 0 || country.Id == null)
                    result = countryService.Create(country);
                else
                    result = countryService.Update(country);

                apiResp.HttpStatusCode = 200;
                apiResp.Data = result;
            }
            else
            {
                apiResp.Data = country;
                apiResp.Data.ValidationErrors = ValidationResult.Errors;
                apiResp.HttpStatusCode = 400;
            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = e.Message;

        }
        return apiResp;

    }
    [HttpPost("delete")]
    public WebApiResponse<CountryModel> Delete(CountryModel country)
    {
        var apiResp = new WebApiResponse<CountryModel>();
        try
        {
            var CountryData = countryService.Delete(country);

            apiResp.HttpStatusCode = 200;
            apiResp.HttpStatusMessage = "Success";
            apiResp.Data = CountryData;
            return apiResp;
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = e.Message;
            return apiResp;
        }

    }
    // [HttpPost("update")]
    // public WebApiResponse<CountryModel> Update(CountryModel country)
    // {
    //     var apiResp = new WebApiResponse<CountryModel>();
    //     try
    //     {
    //         var CountryData = countryService.Update(country);
    //         if (CountryData != null)
    //         {
    //             apiResp.HttpStatusCode = 200;
    //             apiResp.HttpStatusMessage = "Success";
    //             apiResp.Data = CountryData;
    //             return apiResp;
    //         }
    //         else
    //         {
    //             apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
    //             apiResp.HttpStatusMessage = "Problemo in savea in countrie";
    //             return apiResp;
    //         }

    //     }
    //     catch (Exception e)
    //     {
    //         apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
    //         apiResp.HttpStatusMessage = e.Message;
    //         return apiResp;
    //     }

    // }
    [HttpPost("getbyid")]
    public WebApiResponse<CountryModel> GetById(CountryModel country)
    {
        var apiResp = new WebApiResponse<CountryModel>();
        try
        {
            var CountryData = countryService.GetById(country);

            apiResp.HttpStatusCode = 200;
            apiResp.HttpStatusMessage = "Success";
            apiResp.Data = CountryData;
            return apiResp;
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = e.Message;
            return apiResp;
        }

    }
    // [HttpGet("getall")]
    // public WebApiResponse<CountryModel> GetAllCountry()
    // {
    //     var apiResp = new WebApiResponse<CountryModel>();
    //     try
    //     {
    //         var CountryData = countryService.GetAll();

    //         if (CountryData != null)
    //         {
    //             apiResp.DataList = CountryData;
    //             apiResp.HttpStatusCode = 200;
    //             apiResp.HttpStatusMessage = "Success";
    //         }
    //         else
    //         {
    //             apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
    //             apiResp.HttpStatusMessage = "Failure";

    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
    //         apiResp.HttpStatusMessage = e.Message;
    //     }
    //     return apiResp;
    // }
    // [HttpGet("getallCountry")]
    // public IActionResult GetAllCountryList()
    // {
    //     var CountryData = countryService.GetAll();
    //     List<CountryModel> CountryModelData = CountryData.ToList();
    //     return Ok(CountryModelData);
    // }
    [HttpPost("getallcountrydata")]
    public WebApiResponse<BusinessCountryMappingModel> GetAllCountryData(string? search, string? limit, string? offset, string? order, string? dir, string? business, string? country, string? code)
    {
        var apiResp = new WebApiResponse<BusinessCountryMappingModel>();
        try
        {
            var CountryData = countryService.CountryData(search, limit, offset, order, dir, business, country, code);
            if (CountryData.Count() != 0)
            {
                //var JsonDataNull = new { count = 0, results = "", next = "", previous = "" };

                apiResp.Data = null;
                apiResp.DataList = CountryData;
                apiResp.HttpStatusCode = 200;
            }
            else
            {
                apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                apiResp.HttpStatusMessage = "Failure";
            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = e.Message;
        }

        // var JsonData = new { count = CountryData.Count(), results = CountryData, next = "", previous = "" };
        // return Ok(JsonData);
        return apiResp;
    }


}

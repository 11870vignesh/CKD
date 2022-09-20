
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
public class BusinessController : BaseApiController<BusinessController>
{
    private IBusinessService _businessService;

    public BusinessController(ILogger<BusinessController> logger, IMapper mapper, IBusinessService businessService) : base(logger, mapper) => _businessService = businessService;

    [HttpPost("create")]
    public WebApiResponse<BusinessModel> Create(BusinessModel business)
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {
            BusinessValidator BusinessValidation = new BusinessValidator(_businessService);
            var ValidationResult = BusinessValidation.Validate(business);
            if (ValidationResult.IsValid)
            {
                var result = new Business();
                Business businessData = mapper.Map<Business>(business);
                result = _businessService.Add(businessData);

                apiResp.HttpStatusCode = 200;
                apiResp.Data = business;
            }
            else
            {
                apiResp.Data = business;
                apiResp.Data.ValidationErrors = ValidationResult.Errors;
                apiResp.HttpStatusCode = 400;

            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Failure";

        }
        return apiResp;
    }
    [HttpPost("update")]
    public WebApiResponse<BusinessModel> Update(BusinessModel business)
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {
            BusinessValidator BusinessValidation = new BusinessValidator(_businessService);
            var ValidationResult = BusinessValidation.Validate(business);
            if (ValidationResult.IsValid)
            {
                var result = new Business();
                Business businessData = mapper.Map<Business>(business);
                result = _businessService.Update(businessData);

                apiResp.HttpStatusCode = 200;
                apiResp.Data = business;
            }
            else
            {
                apiResp.Data = business;
                apiResp.Data.ValidationErrors = ValidationResult.Errors;
                apiResp.HttpStatusCode = 400;

            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Failure";

        }
        return apiResp;
    }
    [HttpPost("delete")]
    public WebApiResponse<BusinessModel> Delete(BusinessModel business)
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {
            var validator = new BusinessValidator(_businessService);
            var result = validator.Validate(business, options => options.IncludeRuleSets("Delete"));
            if (result.IsValid)
            {
                var BusinessData = _businessService.Delete(mapper.Map<Business>(business));
                if (BusinessData != null)
                {
                    apiResp.Data = business;
                    apiResp.HttpStatusCode = 200;
                    apiResp.HttpStatusMessage = "Success";
                }
                else
                {
                    apiResp.Data = business;
                    apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                    apiResp.HttpStatusMessage = "Failure";
                }
            }
            else
            {
                apiResp.Data = business;
                apiResp.Data.ValidationErrors = result.Errors;
                apiResp.HttpStatusCode = 400;
            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Add failed !(" + e.Message + ")";
        }
        return apiResp;
    }
    [HttpPost("getbyid")]
    public WebApiResponse<BusinessModel> GetById(int? Id)
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {
            var BusinessData = _businessService.GetById(Id);
            if (BusinessData != null)
            {
                BusinessModel businessresult = mapper.Map<BusinessModel>(BusinessData);
                apiResp.HttpStatusCode = 200;
                apiResp.HttpStatusMessage = "Success";
                apiResp.Data = businessresult;
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
        return apiResp;

    }
    [HttpGet("getallbusiness")]
    public WebApiResponse<BusinessModel> GetAllBusinessData()
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {
            var BusinessData = _businessService.GetAllData();
            if (BusinessData != null)
            {
                apiResp.HttpStatusCode = 200;
                apiResp.HttpStatusMessage = "Success";
                apiResp.DataList = mapper.Map<List<BusinessModel>>(BusinessData);
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
        return apiResp;
    }
    [HttpPost("getallbusinessdata")]
    public WebApiResponse<BusinessModel> GetAllBusiness(string? search, string? limit, string? offset, string? order, string? dir, string? business, string? searchdate)
    {
        var apiResp = new WebApiResponse<BusinessModel>();
        try
        {

            List<Business> BusinessData = _businessService.GetAllList(search, limit, offset, order, dir, business);
            List<Business> BusinessDataCount = _businessService.GetAll(search, limit, offset, order, dir, business);
            if (BusinessData != null)
            {
                var BusinessModelData = mapper.Map<List<BusinessModel>>(BusinessData);

                if (BusinessDataCount.Count != 0)
                {
                    BusinessModelData[0].Total = BusinessDataCount.Count();
                }
                apiResp.DataList = BusinessModelData;
                apiResp.HttpStatusCode = 200;
                apiResp.HttpStatusMessage = "Success";
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
        return apiResp;
    }




}
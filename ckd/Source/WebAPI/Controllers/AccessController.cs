using System.Net;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.common;
using Models.entity;
using Models.model;
using WebAPI.DAL;
using WebAPI.Services;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class AccessController : BaseApiController<AccessController>
{
    private IAccessService accessService;

    public AccessController(ILogger<AccessController> logger, IMapper mapper, IAccessService accessSrvc) : base(logger, mapper) => accessService = accessSrvc;


    [HttpGet("get")]
    public WebApiResponse<AccessModel> Get(int Id)
    {
        var apiResp = new WebApiResponse<AccessModel>();
        try
        {
            var entity = accessService.GetById(Id);
            if (entity != null)
            {
                var model = mapper.Map<AccessModel>(entity);
                apiResp.Data = model;
                apiResp.HttpStatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
                apiResp.HttpStatusMessage = "Unable to get data";
            }
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = e.Message;
        }
        return apiResp;
    }

    // Insert or Updating Data into  Access Table .............
    [HttpPost("create")]
    public WebApiResponse<AccessModel> Create(AccessModel data)
    {
        var apiResp = new WebApiResponse<AccessModel>();

        try
        {
            var entity = accessService.Add(mapper.Map<Access>(data));
            apiResp.Data = mapper.Map<AccessModel>(entity);
            apiResp.HttpStatusCode = 200;


        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Add failed !(" + e.Message + ")";
            Log(e.Message);
        }

        return apiResp;

    }

    [HttpPost("GetAll")]
    public WebApiResponse<AccessModel> GetAll(string? search, string? limit, string? offset, string? order, string? dir, string? business)
    {
        var apiResp = new WebApiResponse<AccessModel>();
        try
        {
           List<Access> accessData = accessService.GetAllList(search, limit, offset, order, dir, business);
            List<Access> accessDataCount = accessService.GetAll(search, limit, offset, order, dir, business);
            if (accessData != null)
            {
                var accessModelData = mapper.Map<List<AccessModel>>(accessData);

                if (accessDataCount.Count != 0)
                {
                    accessModelData[0].Total = accessDataCount.Count();
                }
                apiResp.DataList = accessModelData;
            }
        }
        catch (Exception e)
        {
           apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Add failed !(" + e.Message + ")";
            Log(e.Message);
        }
        return apiResp;
    }

    [HttpGet("delete")]
    public WebApiResponse<AccessModel> Delete(int  entityId)
    {
        var apiResp = new WebApiResponse<AccessModel>();
        try
        {
            var result = accessService.Delete(entityId);
            apiResp.HttpStatusCode = 200;
        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Failure";
            Log(e.Message);
        }

        return apiResp;

    }


 [HttpPost("update")]
    public WebApiResponse<AccessModel> Update(AccessModel data)
    {
        var apiResp = new WebApiResponse<AccessModel>();

        try
        {
                 
            var entity = accessService.Update(mapper.Map<Access>(data));
            apiResp.Data = mapper.Map<AccessModel>(entity);

            apiResp.HttpStatusCode = 200;


        }
        catch (Exception e)
        {
            apiResp.HttpStatusCode = (int)HttpStatusCode.InternalServerError;
            apiResp.HttpStatusMessage = "Add failed !(" + e.Message + ")";
            Log(e.Message);
        }

        return apiResp;

    }
}
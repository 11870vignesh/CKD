
using WebAPI.services;
using WebAPI.DAL;
using Models.entity;
using Models.model;
using AutoMapper;
using System.Collections.Generic;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.services;

public interface IBusinessService
{
    Business Add(Business user);
    Business Update(Business business);
    Business GetById(int? Id);
    Business Delete(Business business);
    List<Business> GetAll(string? search, string? limit, string? offset, string? order, string? dir, string? business);
    List<Business> GetAllData();
    List<Business> GetAllList(string? search, string? limit, string? offset, string? order, string? dir, string? business);
    bool CheckBusinessOnCountry(int? Id);
    bool CheckBusiness(int? Id, string business);

}

public class BusinessService : BaseService, IBusinessService
{

    private readonly IMapper _mapper;
    private IRepository<Business> businessRepo;
    private ICountryService _countryService;

    public BusinessService(IMapper mapper, IUnitOfWork uow, ICountryService countryService) : base(uow)
    {
        businessRepo = uow.Repository<Business>();
        _countryService = countryService;
        _mapper = mapper;
    }

    public Business Add(Business business)
    {

        uow.Repository<Business>().Insert(business);
        uow.SaveChanges();
        return business;
    }
    public Business GetById(int? Id)
    {
        var BusinessData = uow.Repository<Business>().GetByID(Id);
        return BusinessData;
    }
    public Business Update(Business business)
    {

        uow.Repository<Business>().Update(business);
        uow.SaveChanges();
        return business;
    }
    public Business Delete(Business business)
    {


        var BusinessData = uow.Repository<Business>().GetQueryAsNoTracking().Where(x => x.Id == business.Id).FirstOrDefault();
        BusinessData.IsActive = false;
        BusinessData.IsDelete = true;
        uow.Repository<Business>().Update(BusinessData);
        uow.SaveChanges();
        return business;

    }
    // public int GetCount()
    // {
    //     int dataCount = uow.Repository<Business>().GetQueryAsNoTracking().Where(x => x.IsActive == true && x.IsDelete == false).Count();
    //     return dataCount;
    // }
    public List<Business> GetAllData()
    {
        List<Business> BusinessList = uow.Repository<Business>().GetQueryAsNoTracking().Where(x => x.IsActive == true && x.IsDelete == false).ToList();
        return BusinessList;
    }
    public List<Business> GetAll(string? search, string? limit, string? offset, string? order, string? dir, string? business)
    {
        List<Business> BusinessList = uow.Repository<Business>().GetQueryAsNoTracking().Where(x => x.IsActive == true && x.IsDelete == false).ToList();

        if (business != "null")
        {
            BusinessList = BusinessList.Where(x => x.Name.Contains(business, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (search != "null")
        {
            BusinessList = BusinessList.Where(x => x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }


        if (order != "Business")
        {
            BusinessList = BusinessList.OrderByDescending(x => x.CreatedOn).ToList();
        }
        else
        {
            if (order == "null" || order == "CreatedOn")
            {
                BusinessList = BusinessList.OrderByDescending(x => x.CreatedOn).ToList();
            }
            else
            {
                if (dir == "null" || dir == "desc")
                {
                    BusinessList = BusinessList.OrderByDescending(x => x.Name).ToList();
                }
                else
                {
                    BusinessList = BusinessList.OrderBy(x => x.Name).ToList();
                }
            }

        }
        return BusinessList;
    }
    public List<Business> GetAllList(string? search, string? limit, string? offset, string? order, string? dir, string? business)
    {
        var BusinessData = GetAll(search, limit, offset, order, dir, business);
        var data = BusinessData.Skip(Convert.ToInt32(offset)).Take(Convert.ToInt32(limit)).ToList();
        return data;
    }
    public bool CheckBusiness(int? Id, string business)
    {
        // if (Id == null)
        // {
        //     Id = 0;
        // }
        // if (Id != 0)
        // {
        // var CheckBusinessValue = uow.Repository<Business>().GetByID(Id);
        // if (business == CheckBusinessValue.Name)
        // {
        //     return true;
        // }
        // else
        // {
        var value = CheckBusinessData(business);
        return value;
        // }
        // }
        // else
        // {
        //     var value = CheckBusinessData(business);
        //     return value;
        // }

    }
    private bool CheckBusinessData(string? business)
    {
        var VerifyBusiness = uow.Repository<Business>().GetQueryAsNoTracking().Where(x => x.Name == business).ToList();
        if (VerifyBusiness.Count() > 0)
            return false;
        else
            return true;
    }
    public bool CheckBusinessOnCountry(int? Id)
    {
        var VerifyBusiness = _countryService.CheckBusinessOnCountry(Id);
        return VerifyBusiness;
    }
}

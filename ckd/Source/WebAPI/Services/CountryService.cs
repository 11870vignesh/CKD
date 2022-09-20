
using WebAPI.services;
using WebAPI.DAL;
using System.IO;
using Models.entity;
using Models.model;
using Models.common;
using AutoMapper;
using System.Collections.Generic;
using System.Security.Principal;
using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.services;

public interface ICountryService
{
    CountryModel Create(CountryModel country);
    CountryModel Update(CountryModel country);
    CountryModel Delete(CountryModel country);
    List<BusinessCountryMappingModel> GetAll();
    CountryModel GetById(CountryModel country);
    List<BusinessCountryMappingModel> GetByBusiness(BusinessCountryMappingModel country);
    List<BusinessCountryMappingModel> CountryData(string? search, string? limit, string? offset, string? order, string? dir, string? business, string? country, string? code);
    bool CheckCountry(int? Id, int? BusinessId, string CountryName);
    bool CheckCountryCode(int? Id, int? BusinessId, string? CountryName, string CountryCode);
    bool CheckBusinessOnCountry(int? Id);
}

public class CountryService : BaseService, ICountryService
{

    //private IJwtUtils _jwtUtils;

    private IRepository<Country> countryRepo;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public CountryService(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment environment) : base(uow)
    {
        //_jwtUtils = jwtUtils;
        countryRepo = uow.Repository<Country>();
        _mapper = mapper;
        _env = environment;
    }

    public CountryModel Create(CountryModel country)
    {
        var CountryprevData = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.IsActive == true).Where(x => x.CountryName == country.CountryName).FirstOrDefault();
        if (CountryprevData != null)
        {
            //var BusinessCountryData = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x=> x.CountryId == CountryprevData.Id).FirstOrDefault();
            BusinessCountryMapping BusinessCountryMapping = new BusinessCountryMapping();
            BusinessCountryMapping.BusinessId = country.BusinessId;
            BusinessCountryMapping.CountryId = CountryprevData.Id;
            BusinessCountryMapping.CreatedOn = DateTime.Now;
            BusinessCountryMapping.IsActive = true;
            BusinessCountryMapping.IsDelete = false;
            BusinessCountryMapping.SortNo = country.SortNo;
            uow.BeginTransaction();
            uow.Repository<BusinessCountryMapping>().Insert(BusinessCountryMapping);
            uow.SaveChanges();
            uow.Commit();
        }
        else
        {

            Country CountryData = new Country();
            CountryData.CountryCode = country.CountryCode;
            CountryData.CountryName = country.CountryName;
            CountryData.IsActive = true;
            CountryData.SortNo = country.SortNo;
            CountryData.CreatedOn = DateTime.Now;
            CountryData.CreatedBy = 1;
            uow.BeginTransaction();
            uow.Repository<Country>().Insert(CountryData);
            uow.SaveChanges();

            BusinessCountryMapping BusinessCountryMapping = new BusinessCountryMapping();
            BusinessCountryMapping.BusinessId = country.BusinessId;
            BusinessCountryMapping.CountryId = CountryData.Id;
            BusinessCountryMapping.CreatedOn = DateTime.Now;
            BusinessCountryMapping.IsActive = true;
            BusinessCountryMapping.IsDelete = false;
            BusinessCountryMapping.SortNo = country.SortNo;
            uow.Repository<BusinessCountryMapping>().Insert(BusinessCountryMapping);
            uow.SaveChanges();

            uow.Commit();
        }
        return country;

    }
    public CountryModel Update(CountryModel country)
    {

        BusinessCountryMapping BusinessCountryMappingData = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.Id == country.BusinessMappingId).FirstOrDefault();

        var CountryData = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.Id == BusinessCountryMappingData.CountryId).FirstOrDefault();

        CountryData.CountryCode = country.CountryCode;
        CountryData.CountryName = country.CountryName;
        CountryData.ModifiedOn = DateTime.Now;

        uow.BeginTransaction();
        uow.Repository<Country>().Update(CountryData);
        uow.SaveChanges();
        if (BusinessCountryMappingData.BusinessId != country.BusinessId)
        {
            BusinessCountryMappingData.BusinessId = country.BusinessId;
            BusinessCountryMappingData.ModifiedOn = DateTime.Now;
            uow.Repository<BusinessCountryMapping>().Update(BusinessCountryMappingData);
            uow.SaveChanges();
        }
        uow.Commit();
        return country;

    }
    public CountryModel Delete(CountryModel country)
    {

        BusinessCountryMapping BusinessCountryData = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.Id == country.BusinessMappingId).FirstOrDefault();
        BusinessCountryData.IsActive = false;
        BusinessCountryData.IsDelete = true;
        uow.BeginTransaction();
        uow.Repository<BusinessCountryMapping>().Update(BusinessCountryData);
        uow.SaveChanges();
        BusinessCountryMapping NumberofCountryData = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.CountryId == BusinessCountryData.CountryId).Where(x => x.IsActive == true).FirstOrDefault();
        if (NumberofCountryData == null)
        {
            Country CountryData = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.Id == BusinessCountryData.CountryId).FirstOrDefault();
            CountryData.IsActive = false;
            CountryData.IsDelete = true;
            uow.Repository<Country>().Update(CountryData);
            uow.SaveChanges();
        }
        uow.Commit();
        return country;
    }
    public CountryModel GetById(CountryModel country)
    {
        var BusinessCountryMapData = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.Id == country.BusinessMappingId).FirstOrDefault();
        var CountryData = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.Id == BusinessCountryMapData.CountryId).FirstOrDefault();
        CountryModel CountryValue = new CountryModel();
        CountryValue.BusinessId = BusinessCountryMapData.BusinessId;
        CountryValue.BusinessMappingId = BusinessCountryMapData.Id;
        CountryValue.Id = CountryData.Id;
        CountryValue.CountryCode = CountryData.CountryCode;
        CountryValue.CountryName = CountryData.CountryName;
        // CountryData.BusinessId = BusinessCountryMapData.BusinessId;
        // CountryData.BusinessMappingId = BusinessCountryMapData.Id;
        return CountryValue;

    }
    public List<BusinessCountryMappingModel> GetByBusiness(BusinessCountryMappingModel country)
    {
        List<BusinessCountryMappingModel> CountryData = uow.Repository<BusinessCountryMapping>()
                                     .GetQueryAsNoTracking(x => x.IsActive == true)
                                     .Include(I => I.Business)
                                     .Include(R => R.Country)
                                     .Where(x => x.BusinessId == country.BusinessId)
                                    .Select(s => new BusinessCountryMappingModel()
                                    {
                                        Id = s.Id,
                                        CountryName = s.Country.CountryName,
                                        SortNo = s.SortNo,
                                        BusinessId = s.BusinessId,
                                        CountryCode = s.Country.CountryCode,
                                        CountryId = s.Country.Id,


                                        BusinessName = s.Business.Name,


                                        CreatedBy = s.CreatedBy,
                                        ModifiedBy = s.ModifiedBy,
                                        CreatedOn = s.CreatedOn,
                                        ModifiedOn = s.ModifiedOn,
                                        IsActive = s.IsActive,

                                    }).ToList();

        return CountryData;

    }
    public List<BusinessCountryMappingModel> GetAll()
    {
        // var CountryData = _mapper.Map<List<CountryModel>>
        // (uow.Repository<Country>().GetQueryAsNoTracking(x => x.is_active == true).ToList());


        List<BusinessCountryMappingModel> CountryData = uow.Repository<BusinessCountryMapping>()
                                     .GetQueryAsNoTracking(x => x.IsActive == true)
                                     .Include(I => I.Business)
                                     .Include(R => R.Country)
                                    .Select(s => new BusinessCountryMappingModel()
                                    {
                                        Id = s.Id,
                                        CountryName = s.Country.CountryName,
                                        SortNo = s.SortNo,
                                        BusinessId = s.BusinessId,
                                        CountryCode = s.Country.CountryCode,
                                        CountryId = s.Country.Id,


                                        BusinessName = s.Business.Name,


                                        CreatedBy = s.CreatedBy,
                                        ModifiedBy = s.ModifiedBy,
                                        CreatedOn = s.CreatedOn,
                                        ModifiedOn = s.ModifiedOn,
                                        IsActive = s.IsActive,

                                    }).ToList();


        return CountryData;



    }
    public List<BusinessCountryMappingModel> CountryData(string? search, string? limit, string? offset, string? order, string? dir, string? business, string? country, string? code)
    {

        List<BusinessCountryMappingModel> CountryData = uow.Repository<BusinessCountryMapping>()
                                    .GetQueryAsNoTracking(x => x.IsActive == true)
                                    .Include(I => I.Business)
                                    .Include(R => R.Country)
                                   .Select(s => new BusinessCountryMappingModel()
                                   {
                                       Id = s.Id,
                                       CountryName = s.Country.CountryName,
                                       SortNo = s.SortNo,
                                       BusinessId = s.BusinessId,
                                       CountryCode = s.Country.CountryCode,
                                       CountryId = s.Country.Id,


                                       BusinessName = s.Business.Name,


                                       CreatedBy = s.CreatedBy,
                                       ModifiedBy = s.ModifiedBy,
                                       CreatedOn = s.CreatedOn,
                                       ModifiedOn = s.ModifiedOn,
                                       IsActive = s.IsActive,

                                   }).ToList();

        // var searchType = !(String.IsNullOrEmpty(search));
        // Console.WriteLine(searchType);
        // bool searchvalue;
        // try
        // {
        //     searchvalue = Convert.ToBoolean(search);
        // }
        // catch
        // {
        //     searchvalue = false;
        // }

        // if (country != "null" || country != "0")
        // {
        //     CountryData = CountryData.Where(x => x.Id == Convert.ToInt32(country)).ToList();
        // }
        if (business != "null")
        {
            // CountryData = CountryData.Where(x => x.BusinessName == business).ToList();
            CountryData = CountryData.Where(x => x.BusinessName.Contains(business, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (country != "null")
        {
            // CountryData = CountryData.Where(x => x.CountryName == country).ToList();
            CountryData = CountryData.Where(x => x.CountryName.Contains(country, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (code != "null")
        {
            // CountryData = CountryData.Where(x => x.CountryCode == code).ToList();
            CountryData = CountryData.Where(x => x.CountryCode.Contains(code, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (search != "null")
        {
            CountryData = CountryData.Where(x => x.CountryCode.Contains(search) || x.CountryName.Contains(search) || x.BusinessName.Contains(search)).ToList();
        }

        if (order == "country_code")
        {
            if (dir == "null" || dir == "undefined" || dir == "asc")
            {
                CountryData = CountryData.OrderBy(x => x.CountryCode).ToList();
            }
            else
            {
                CountryData = CountryData.OrderByDescending(x => x.CountryCode).ToList();
            }

        }
        else if (order == "country_name")
        {
            if (dir == "null" || dir == "undefined" || dir == "asc")
            {
                CountryData = CountryData.OrderBy(x => x.CountryName).ToList();
            }
            else
            {
                CountryData = CountryData.OrderByDescending(x => x.CountryName).ToList();
            }
        }
        else
        {
            if (dir == "null" || dir == "undefined" || dir == "asc")
            {
                CountryData = CountryData.OrderBy(x => x.BusinessName).ToList();
            }
            else
            {
                CountryData = CountryData.OrderByDescending(x => x.BusinessName).ToList();
            }
        }


        var data = CountryData.Skip(Convert.ToInt32(offset)).Take(Convert.ToInt32(limit)).ToList();

        return data;
    }
    public bool CheckCountry(int? Id, int? BusinessId, string CountryName)
    {
        if (Id == null)
        {
            Id = 0;
        }
        if (Id != 0)
        {
            var CheckCountryValue = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.Id == Id).FirstOrDefault();
            if (CheckCountryValue != null)
            {
                var CheckCountryName = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.Id == CheckCountryValue.CountryId).FirstOrDefault();
                if (CountryName == CheckCountryName.CountryName)
                {
                    return true;
                }
                else
                {
                    var value = CheckCountryData(BusinessId, CountryName);
                    return value;
                }
            }
            else
            {
                var value = CheckCountryData(BusinessId, CountryName);
                return value;
            }

        }
        else
        {
            var value = CheckCountryData(BusinessId, CountryName);
            return value;
        }

    }
    private bool CheckCountryData(int? BusinessId, string? CountryName)
    {
        Country VerifyCountry = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.CountryName == CountryName).FirstOrDefault();

        if (VerifyCountry != null)
        {
            var VerifyCountryBusinessMatch = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.BusinessId == BusinessId).Where(x => x.CountryId == VerifyCountry.Id).FirstOrDefault();
            if (VerifyCountryBusinessMatch != null)
                return false;
            else
                return true;
        }
        else
        {
            return true;
        }

    }

    public bool CheckCountryCode(int? Id, int? BusinessId, string? CountryName, string CountryCode)
    {
        if (Id == null)
        {
            Id = 0;
        }
        if (Id != 0)
        {
            var CheckCountryValue = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.Id == Id).FirstOrDefault();
            if (CountryCode == CheckCountryValue.CountryCode)
            {
                return true;
            }
            else
            {
                var value = CheckCountryCodeData(CountryCode, CountryName, BusinessId);
                return value;
            }
        }
        else
        {
            var value = CheckCountryCodeData(CountryCode, CountryName, BusinessId);
            return value;
        }

    }
    private bool CheckCountryCodeData(string? CountryCode, string? CountryName, int? BusinessId)
    {

        var VerifyCountry = uow.Repository<Country>().GetQueryAsNoTracking().Where(x => x.CountryCode == CountryCode).Where(x => x.IsActive == true).FirstOrDefault();
        if (VerifyCountry != null)
        {
            //var VerifyCode = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x=> x.CountryId == VerifyCountry.BusinessId).FirstOrDefault();
            if (VerifyCountry.CountryName == CountryName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return true;
    }
    public bool CheckBusinessOnCountry(int? Id)
    {
        var VerifyBusiness = uow.Repository<BusinessCountryMapping>().GetQueryAsNoTracking().Where(x => x.BusinessId == Id).Any();
        if (VerifyBusiness == false)
            return true;
        else
            return false;
    }

}
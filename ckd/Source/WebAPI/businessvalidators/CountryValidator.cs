using FluentValidation.AspNetCore;
using FluentValidation;
using WebAPI.services;
using Models.entity;
using Models.model;
using WebAPI.DAL;
using AutoMapper;

namespace Models.model;

public class CountryValidator : AbstractValidator<CountryModel>
{
    private ICountryService _countryService;
    private readonly IMapper _mapper;
    private IRepository<Country> countryRepo;
    public CountryValidator(ICountryService countryService)
    {
        _countryService = countryService;

        // RuleFor(x => x.CountryName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("Country Name should be not empty");
        // RuleFor(x => x.CountryCode).Cascade(CascadeMode.StopOnFirstFailure).NotNull().WithMessage("Country Code should be not empty");

        //RuleFor(x => x.BusinessId).Cascade(CascadeMode.StopOnFirstFailure).Must(IsNullInt).WithMessage("Business should be not empty");
        RuleFor(x => x.BusinessId).Cascade(CascadeMode.StopOnFirstFailure)
                        .NotNull().WithMessage("Business should be not empty");


        RuleFor(x => x.CountryName).Cascade(CascadeMode.StopOnFirstFailure)
                        .Must(IsNullString).WithMessage("Country Name should be not empty")
                        .Must(IsValidCountryName).WithMessage("Name should be greater than 3 characters and less than 50 Characters")
                        .Must((country, CountryName) => NotSameCountry(country.BusinessMappingId, country.BusinessId, CountryName)).WithMessage("Country Name Should not be same");


        RuleFor(x => x.CountryCode).Cascade(CascadeMode.StopOnFirstFailure)
                        .Must(IsNullString).WithMessage("Country Code should be not empty")
                        .Must(IsValidCountryCode).WithMessage("Code should be greater than 2 characters and less than 3 Characters")
                        .Must((country, CountryCode) => NotSameCountryCode(country.Id, country.BusinessId, country.CountryName, CountryCode)).WithMessage("Country Code Should not be same");

        //RuleFor(x => x.CountryName).Must(IsValidCountryName).WithMessage("Name should be greater than 3 characters and less than 50 Characters");
        //RuleFor(x => x.CountryCode).Must(IsValidCountryCode).WithMessage("Name should be greater than 2 characters and less than 3 Characters");

        //RuleFor(x => x.Name).Must(NotSame).WithMessage("Business Name Should not be same");

        //RuleFor(m => m.CountryName).Must((country, CountryName) => NotSameCountry(country.Id, CountryName)).WithMessage("Business Name Should not be same");
        //RuleFor(m => m.CountryCode).Must((country, CountryCode) => NotSameCountryCode(country.Id, CountryCode)).WithMessage("Business Name Should not be same");
    }
    // public BusinessValidator(IBusinessService businessService)
    // {
    //     _businessService = businessService;
    // }
    private bool IsNullString(string pass_)
    {
        if (pass_ == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool IsValidCountryCode(string pass_)
    {



        if (pass_.Length == 2 || pass_.Length == 3)
        {
            return true;

        }
        else
        {
            return false;
        }

    }
    private bool IsValidCountryName(string pass_)
    {
        if (pass_.Length > 3 && pass_.Length < 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool NotSameCountry(int? Id, int? BusinessId, string CountryName)
    {
        if (BusinessId != null)
        {
            var CountryValue = _countryService.CheckCountry(Id, BusinessId, CountryName);
            return CountryValue;
        }
        else
        {
            return false;
        }



    }
    private bool NotSameCountryCode(int? Id, int? BusinessId, string? CountryName, string CountryCode)
    {
        if (BusinessId != null)
        {
            var CountryValue = _countryService.CheckCountryCode(Id, BusinessId, CountryName, CountryCode);
            return CountryValue;
        }
        else
        {
            return false;
        }


    }
}

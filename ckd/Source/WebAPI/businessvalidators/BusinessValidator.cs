using FluentValidation.AspNetCore;
using FluentValidation;
using WebAPI.services;
using Models.entity;
using WebAPI.DAL;
using AutoMapper;
using Models.model;


namespace Models.model;

public class BusinessValidator : AbstractValidator<BusinessModel>
{
    private IBusinessService _businessService;
    private readonly IMapper _mapper;
    private IRepository<Business> businessRepo;
    public BusinessValidator(IBusinessService businessService)
    {
        _businessService = businessService;
        RuleFor(x => x.Name).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Name should be not empty").Must(IsValidName).WithMessage("Name should be greater than 3 characters.").Must((business, Name) => NotSame(business.Id, Name)).WithMessage("Business Name Should not be same");

        RuleSet("Delete", () =>
            {
                RuleFor(x => x.Id).Must((business, Name) => CheckBusiness(business.Id)).WithMessage("Business Assigned with Countries");
            });

        //RuleFor(x => x.Name).Must(NotSame).WithMessage("Business Name Should not be same");

    }
    // public BusinessValidator(IBusinessService businessService)
    // {
    //     _businessService = businessService;
    // }
    private bool IsValidName(string pass_)
    {
        if (pass_.Length < 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool NotSame(int? Id, string business)
    {

        var businessvalue = _businessService.CheckBusiness(Id, business);
        return businessvalue;


    }
    private bool CheckBusiness(int? Id)
    {
        var businessvalue = _businessService.CheckBusinessOnCountry(Id);
        return businessvalue;
    }


}
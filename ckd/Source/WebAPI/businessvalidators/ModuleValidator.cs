// using FluentValidation.AspNetCore;
// using FluentValidation;
// using WebAPI.services;
// using Models.entity;
// using WebAPI.DAL;
// using AutoMapper;
// using Models.model;


// namespace Models.model;

// public class ModuleValidator : AbstractValidator<ModuleModel>
// {
//     private IModuleService _moduleService;
//     private readonly IMapper _mapper;
//     private IRepository<Module> moduleRepo;
//     public ModuleValidator(IModuleService moduleService)
//     {
//         _moduleService = moduleService;

//         RuleFor(x => x.Name).Cascade(CascadeMode.StopOnFirstFailure).Must(IsNullString).WithMessage("Module Name should be not empty").Must(IsValidModuleName).WithMessage("Name should be greater than 5 characters and less than 50 Characters").Must((module, Name) => NotSameModule(module.Id, Name)).WithMessage("Module Name Should not be same");
//         RuleFor(x => x.Description).Cascade(CascadeMode.StopOnFirstFailure).Must(IsNullString).WithMessage("Module Description should be not empty").Must(IsValidDescription).WithMessage("Name should be greater than 10 characters and less than 150 Characters");
//     }
//     // public BusinessValidator(IBusinessService businessService)
//     // {
//     //     _businessService = businessService;
//     // }
//     private bool IsNullString(string pass_)
//     {
//         if (pass_ != null)
//         {
//             return true;
//         }
//         else
//         {
//             return false;
//         }
//     }
//     private bool IsValidModuleName(string pass_)
//     {


//         if (pass_.Length < 5 || pass_.Length > 50)
//         {
//             return false;
//         }
//         else
//         {
//             return true;
//         }
//     }
//     private bool IsValidDescription(string pass_)
//     {


//         if (pass_.Length < 10 || pass_.Length > 150)
//         {
//             return false;
//         }
//         else
//         {
//             return true;
//         }
//     }
//     private bool NotSameModule(int? Id, string module)
//     {

//         var modulevalue = _moduleService.CheckModule(Id, module);
//         return modulevalue;


//     }
// }
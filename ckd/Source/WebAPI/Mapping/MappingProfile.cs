using AutoMapper;
using Models.entity;
using Models.model;

namespace WebAPI.mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {

        // var config = new MapperConfiguration(cfg => {
        //       cfg.CreateMap<Order, OrderDTO>()
        //           //OrderId is different so map them using For Member
        //           .ForMember(dest => dest.OrderId, act => act.MapFrom(src => src.OrderNo))
        //           //Customer is a Complex type, so Map Customer to Simple type using For Member
        //           .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Customer.FullName))
        //           .ForMember(dest => dest.Postcode, act => act.MapFrom(src => src.Customer.Postcode))
        //           .ForMember(dest => dest.MobileNo, act => act.MapFrom(src => src.Customer.ContactNo))
        //           .ForMember(dest => dest.CustomerId, act => act.MapFrom(src => src.Customer.CustomerID))
        //           .ReverseMap();
        //   });

        //  CreateMap<User, UserModel>().ReverseMap();
        CreateMap<Business, BusinessModel>().ReverseMap();
        CreateMap<Country, CountryModel>().ReverseMap();
             CreateMap<Access ,AccessModel>().ReverseMap();

        //CreateMap<Module, ModuleModel>().ReverseMap();

    }
}
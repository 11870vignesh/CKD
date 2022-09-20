using AutoMapper;
using WebAPI.DAL;

namespace WebAPI.services;
public class BaseService
{
    protected readonly IMapper mapper;
    protected IUnitOfWork uow { get; set; }

    //private IEmailService _emailService;
    //private IUtilityService _utilityService;

    public BaseService(IUnitOfWork uow)
    {
        this.uow = uow;
        this.mapper = mapper;
    }


}
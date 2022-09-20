using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers;

//[Authorize]
[ApiController]
public class BaseApiController<T> : Controller
{
    protected ILogger<T> _logger;
    protected readonly IMapper mapper;

    public BaseApiController(ILogger<T> logger, IMapper mapper)
    {
        _logger = logger;
        this.mapper = mapper;
    }

    public void Log(string message)
    {
        //_logger.Log(LogLevel.Debug, 0, )

    }

    [HttpGet("version")]
    public string Version()
    {
        return "WebAPI version 1.1a;";
    }

    [HttpGet("server_datetime")]
    public string ServerDT()
    {
        return "Server Time Now: IST " + DateTime.Now.ToLongDateString();
    }

}

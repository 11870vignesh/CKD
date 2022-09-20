using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers;

public class HomeController : BaseController<HomeController>
{

    public HomeController(ILogger<HomeController> logger, IConfiguration config) : base(logger, config)
    {

    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult ErrorPage()
    {
        return PartialView("_404_ErrorPage");
    }


    public IActionResult Master()
    {
        return View();
    }



    public IActionResult Error()
    {
        return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

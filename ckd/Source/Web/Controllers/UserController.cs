
using Microsoft.AspNetCore.Authorization;
using Models.entity;
namespace Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Models.entity;

public class UserController : BaseController<UserController>
{

    public UserController(ILogger<UserController> logger, IConfiguration config) : base(logger, config)
    {

    }

    public IActionResult Login()
    {
        return View();
    }

    // public async Task<IActionResult> UserRegistration()
    // {
    //     try
    //     {
    //         var userModelUrl = "user/GetAllUserModel";

    //         var userModelList = await GetRequest<WebApiResponse<UserListModel>>(userModelUrl);

    //         if (userModelList.HttpStatusCode == 200)
    //         {
    //             UserListModel modelData = new UserListModel();

    //             foreach (var item in userModelList.DataList)
    //             {
    //                 modelData.DepartmentList = item.DepartmentList;
    //                 modelData.BusinessList = item.BusinessList;
    //                 modelData.CountryList = item.CountryList;
    //                 modelData.RolesList = item.RolesList;
    //                 modelData.AccessList = item.AccessList;
    //                 //modelData.ModuleId = (int[])Enum.GetValues(typeof(UserListModel.Modules));
    //             }
    //             return View(modelData);
    //         }
    //         else
    //         {
    //             return View();
    //         }
    //     }

    //     catch (Exception e)
    //     {
    //         return View();
    //     }


    // }

    public IActionResult UserRegistration()
    {
        return View();
    }

}
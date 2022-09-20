using Models.entity;
namespace Models.model;

public class UserModel : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string EmailId { get; set; }
    public string? PhoneNumber { get; set; }
    public int ChangeManagementRoleId { get; set; }
    public int ChangeManagementAccessId { get; set; }
    public int AssemblyDocumentsRoleId { get; set; }
    public int AssemblyDocumentsAccessId { get; set; }
    public int QFLRoleId { get; set; }
    public int QFLAccessId { get; set; }
    public int LotRoleId { get; set; }
    public int LotAccessId { get; set; }

}

public class UserListModel : BaseModel
{
    public List<DepartmentModel> DepartmentList { get; set; }
    public List<BusinessModel> BusinessList { get; set; }
    public List<CountryModel> CountryList { get; set; }
    public List<RolesListModel> RolesList { get; set; }
    public List<AccessModel> AccessList { get; set; }

}

public class RolesListModel : BaseModel
{
    public List<RolesModel> ChangeManagementRolesList { get; set; }
    public List<RolesModel> AssemblyDocsRolesList { get; set; }
    public List<RolesModel> QFLRolesList { get; set; }
    public List<RolesModel> LotRolesList { get; set; }
    public enum Modules
    {
        Change_Management = 1,
        Assembly_Documents = 2,
        QFL_Documents = 6,
        LOT_Information = 7

    }

}





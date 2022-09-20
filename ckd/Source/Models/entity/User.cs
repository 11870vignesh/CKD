namespace Models.entity;

using System.Text.Json.Serialization;

public class User : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string EmailId { get; set; }
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public int BusinessId { get; set; }
    public int CountryId { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsRegistered { get; set; }
    public DateTime LastLoggedIn { get; set; }
}
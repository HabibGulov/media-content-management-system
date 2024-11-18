public class User : BaseEntity
{
    public Guid RoleId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password {get; set;} = string.Empty;

    public Role Role { get; set; } = default!;
}
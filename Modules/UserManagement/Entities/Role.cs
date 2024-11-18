public class Role : BaseEntity
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public Permission Permission { get; set; } = default!;
}
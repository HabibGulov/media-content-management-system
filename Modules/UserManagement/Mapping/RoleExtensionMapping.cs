public static class RoleMappingExtensions
{
    public static RoleReadInfo ToReadInfo(this Role role)
    {
        return new RoleReadInfo()
        {
            Id = role.Id,
            BaseInfo = new RoleBaseInfo()
            {
                PermissionId = role.PermissionId,
                Name = role.Name,
                Description = role.Description
            }
        };
    }

    public static Role ToRole(this RoleCreateInfo roleCreateInfo)
    {
        return new Role()
        {
            PermissionId = roleCreateInfo.BaseInfo.PermissionId,
            Name = roleCreateInfo.BaseInfo.Name,
            Description = roleCreateInfo.BaseInfo.Description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Role UpdateRole(this Role role, RoleUpdateInfo roleUpdateInfo)
    {
        role.PermissionId = roleUpdateInfo.BaseInfo.PermissionId;
        role.Name = roleUpdateInfo.BaseInfo.Name;
        role.Description = roleUpdateInfo.BaseInfo.Description;
        role.UpdatedAt = DateTime.UtcNow;
        role.Version += 1;
        return role;
    }

    public static Role DeleteRole(this Role role)
    {
        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;
        role.UpdatedAt = DateTime.UtcNow;
        role.Version += 1;
        return role;
    }

    public static Role FromUpdateToRole(this RoleUpdateInfo roleUpdateInfo)
    {
        return new Role()
        {
            PermissionId = roleUpdateInfo.BaseInfo.PermissionId,
            Name = roleUpdateInfo.BaseInfo.Name,
            Description = roleUpdateInfo.BaseInfo.Description
        };
    }
}

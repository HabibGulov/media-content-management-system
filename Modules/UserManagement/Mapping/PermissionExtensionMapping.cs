public static class PermissionMappingExtensions
{
    public static PermissionReadInfo ToReadInfo(this Permission permission)
    {
        return new PermissionReadInfo()
        {
            Id = permission.Id,
            BaseInfo = new PermissionBaseInfo()
            {
                Name = permission.Name,
                Description = permission.Description
            }
        };
    }

    public static Permission ToPermission(this PermissionCreateInfo permissionCreateInfo)
    {
        return new Permission()
        {
            Name = permissionCreateInfo.BaseInfo.Name,
            Description = permissionCreateInfo.BaseInfo.Description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Permission UpdatePermission(this Permission permission, PermissionUpdateInfo permissionUpdateInfo)
    {
        permission.Name = permissionUpdateInfo.BaseInfo.Name;
        permission.Description = permissionUpdateInfo.BaseInfo.Description;
        permission.UpdatedAt = DateTime.UtcNow;
        permission.Version += 1;
        return permission;
    }

    public static Permission DeletePermission(this Permission permission)
    {
        permission.IsDeleted = true;
        permission.DeletedAt = DateTime.UtcNow;
        permission.UpdatedAt = DateTime.UtcNow;
        permission.Version += 1;
        return permission;
    }

    public static Permission FromUpdateToPermission(this PermissionUpdateInfo permissionUpdateInfo)
    {
        return new Permission()
        {
            Name = permissionUpdateInfo.BaseInfo.Name,
            Description = permissionUpdateInfo.BaseInfo.Description
        };
    }
}

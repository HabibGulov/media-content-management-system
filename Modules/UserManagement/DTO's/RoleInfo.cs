public readonly record struct RoleBaseInfo(
    int PermissionId,
    string Name,
    string? Description
);

public readonly record struct RoleCreateInfo(
    RoleBaseInfo BaseInfo
);

public readonly record struct RoleUpdateInfo(
    RoleBaseInfo BaseInfo
);

public readonly record struct RoleReadInfo(
    Guid Id,
    RoleBaseInfo BaseInfo
);

public readonly record struct PermissionBaseInfo(
    string Name,
    string? Description
);

public readonly record struct PermissionCreateInfo(
    PermissionBaseInfo BaseInfo
);

public readonly record struct PermissionUpdateInfo(
    PermissionBaseInfo BaseInfo
);

public readonly record struct PermissionReadInfo(
    Guid Id,
    PermissionBaseInfo BaseInfo
);

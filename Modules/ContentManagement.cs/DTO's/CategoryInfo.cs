public readonly record struct CategoryBaseInfo(
    string Name,
    string Description
);

public readonly record struct CategoryCreateInfo(
    CategoryBaseInfo BaseInfo
);

public readonly record struct CategoryReadInfo(
    Guid Id,
    CategoryBaseInfo BaseInfo
);

public readonly record struct CategoryUpdateInfo(
    CategoryBaseInfo BaseInfo
);

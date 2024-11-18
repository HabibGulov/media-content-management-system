public readonly record struct ContentCategoryBaseInfo(
    Guid CategoryId,
    Guid ContentId
);

public readonly record struct ContentCategoryCreateInfo(
    ContentCategoryBaseInfo BaseInfo
);

public readonly record struct ContentCategoryReadInfo(
    Guid Id,
    ContentCategoryBaseInfo BaseInfo
);

public readonly record struct ContentCategoryUpdateInfo(
    ContentCategoryBaseInfo BaseInfo
);
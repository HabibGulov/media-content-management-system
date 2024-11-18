public readonly record struct ContentBaseInfo(
    string Title,
    string Description,
    string Url,
    bool IsFree,
    double Price,
    Guid UserId,
    ContentType ContentType
);

public readonly record struct ContentCreateInfo(
    ContentBaseInfo BaseInfo,
    IFormFile File
);

public readonly record struct ContentReadInfo(
    Guid Id,
    ContentBaseInfo BaseInfo,
    IFormFile File
);

public readonly record struct ContentUpdateInfo(
    ContentBaseInfo BaseInfo,
    IFormFile File
);
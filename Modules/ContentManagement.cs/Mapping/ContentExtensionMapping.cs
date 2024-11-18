public static class ContentMappingExtensions
{
    public static ContentReadInfo ToReadInfo(this Content content)
    {
        return new ContentReadInfo()
        {
            Id = content.Id,
            BaseInfo = new ContentBaseInfo(
                content.Title,
                content.Description,
                content.Url,
                content.IsFree,
                content.Price ?? 0,
                content.UserId,
                content.ContentType
            ),
        };
    }

    public static Content ToContent(this ContentCreateInfo contentCreateInfo)
    {
        return new Content()
        {
            Title = contentCreateInfo.BaseInfo.Title,
            Description = contentCreateInfo.BaseInfo.Description,
            Url = contentCreateInfo.BaseInfo.Url,
            IsFree = contentCreateInfo.BaseInfo.IsFree,
            Price = contentCreateInfo.BaseInfo.Price,
            UserId = contentCreateInfo.BaseInfo.UserId,
            ContentType = contentCreateInfo.BaseInfo.ContentType,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Content UpdateContent(this Content content, ContentUpdateInfo contentUpdateInfo)
    {
        content.Title = contentUpdateInfo.BaseInfo.Title;
        content.Description = contentUpdateInfo.BaseInfo.Description;
        content.Url = contentUpdateInfo.BaseInfo.Url;
        content.IsFree = contentUpdateInfo.BaseInfo.IsFree;
        content.Price = contentUpdateInfo.BaseInfo.Price;
        content.UserId = contentUpdateInfo.BaseInfo.UserId;
        content.ContentType = contentUpdateInfo.BaseInfo.ContentType;
        content.UpdatedAt = DateTime.UtcNow;
        content.Version += 1;
        return content;
    }

    public static Content DeleteContent(this Content content)
    {
        content.IsDeleted = true;
        content.DeletedAt = DateTime.UtcNow;
        content.UpdatedAt = DateTime.UtcNow;
        content.Version += 1;
        return content;
    }
}

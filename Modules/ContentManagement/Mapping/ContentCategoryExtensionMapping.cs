public static class ContentCategoryMappingExtensions
{
    public static ContentCategoryReadInfo ToReadInfo(this ContentCategory contentCategory)
    {
        return new ContentCategoryReadInfo()
        {
            Id = contentCategory.Id,
            BaseInfo = new ContentCategoryBaseInfo(
                contentCategory.CategoryId,
                contentCategory.ContentId
            )
        };
    }

    public static ContentCategory ToContentCategory(this ContentCategoryCreateInfo contentCategoryCreateInfo)
    {
        return new ContentCategory()
        {
            CategoryId = contentCategoryCreateInfo.BaseInfo.CategoryId,
            ContentId = contentCategoryCreateInfo.BaseInfo.ContentId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static ContentCategory UpdateContentCategory(this ContentCategory contentCategory, ContentCategoryUpdateInfo contentCategoryUpdateInfo)
    {
        contentCategory.CategoryId = contentCategoryUpdateInfo.BaseInfo.CategoryId;
        contentCategory.ContentId = contentCategoryUpdateInfo.BaseInfo.ContentId;
        contentCategory.UpdatedAt = DateTime.UtcNow;
        contentCategory.Version += 1;
        return contentCategory;
    }

    public static ContentCategory DeleteContentCategory(this ContentCategory contentCategory)
    {
        contentCategory.IsDeleted = true;
        contentCategory.DeletedAt = DateTime.UtcNow;
        contentCategory.UpdatedAt = DateTime.UtcNow;
        contentCategory.Version += 1;
        return contentCategory;
    }

    public static ContentCategory FromUpdateToContentCategory(this ContentCategoryUpdateInfo info)
    {
        return new ContentCategory()
        {

        };
    }
}

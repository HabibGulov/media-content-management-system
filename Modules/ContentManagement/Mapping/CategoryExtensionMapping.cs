public static class CategoryMappingExtensions
{
    public static CategoryReadInfo ToReadInfo(this Category category)
    {
        return new CategoryReadInfo()
        {
            Id = category.Id,
            BaseInfo = new CategoryBaseInfo()
            {
                Name = category.Name,
                Description = category.Description!
            }
        };
    }

    public static Category ToCategory(this CategoryCreateInfo categoryCreateInfo)
    {
        return new Category()
        {
            Name = categoryCreateInfo.BaseInfo.Name,
            Description = categoryCreateInfo.BaseInfo.Description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Category UpdateCategory(this Category category, CategoryUpdateInfo categoryUpdateInfo)
    {
        category.Name = categoryUpdateInfo.BaseInfo.Name;
        category.Description = categoryUpdateInfo.BaseInfo.Description;
        category.UpdatedAt = DateTime.UtcNow;
        category.Version += 1;
        return category;
    }

    public static Category DeleteCategory(this Category category)
    {
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;
        category.Version += 1;
        return category;
    }

    public static Category FromUpdateToCategory(this CategoryUpdateInfo categoryUpdateInfo)
    {
        return new Category()
        {
            Name = categoryUpdateInfo.BaseInfo.Name,
            Description = categoryUpdateInfo.BaseInfo.Description
        };
    }
}

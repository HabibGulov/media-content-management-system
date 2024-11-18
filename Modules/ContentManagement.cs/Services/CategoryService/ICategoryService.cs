public interface ICategoryService
{
    Task<Result<bool>> AddCategory(CategoryCreateInfo categoryCreateInfo);
    Task<Result<CategoryReadInfo>> GetCategoryById(Guid id);
    Task<Result<PagedResponse<IEnumerable<CategoryReadInfo>>>> GetCategories(CategoryFilter filter);
    Task<Result<bool>> UpdateCategory(Guid id, CategoryUpdateInfo categoryUpdateInfo);
    Task<Result<bool>> DeleteCategory(Guid id);
}
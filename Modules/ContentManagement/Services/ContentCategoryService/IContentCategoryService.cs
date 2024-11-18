public interface IContentCategoryService
{
    Task<Result<bool>> AddContentCategory(ContentCategoryCreateInfo contentCategoryCreateInfo);
    Task<Result<ContentCategoryReadInfo>> GetContentCategoryById(Guid id);
    Task<Result<PagedResponse<IEnumerable<ContentCategoryReadInfo>>>> GetContentCategories(ContentCategoryFilter filter);
    Task<Result<bool>> UpdateContentCategory(Guid id, ContentCategoryUpdateInfo contentCategoryUpdateInfo);
    Task<Result<bool>> DeleteContentCategory(Guid id);
}
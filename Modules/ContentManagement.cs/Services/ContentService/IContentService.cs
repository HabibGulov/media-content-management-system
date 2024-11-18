public interface IContentService
{
    Task<Result<ContentReadInfo>> GetContentById(Guid contentId);
    Task<Result<List<ContentReadInfo>>> GetAllContents(ContentFilter filter);
    Task<Result<bool>> CreateContent(ContentCreateInfo contentCreateInfo, Guid userId);
    Task<Result<bool>> UpdateContent(Guid contentId, ContentUpdateInfo contentUpdateInfo, Guid userId);
    Task<Result<bool>> DeleteContent(Guid contentId, Guid userId);
    Task<string> GetContentUrl(Guid contentId);  
}
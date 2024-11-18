using System.Linq.Expressions;

public class ContentCategoryService : IContentCategoryService
{
    private readonly IUnitOfWork<ContentCategory> unitOfWork;

    public ContentCategoryService(IUnitOfWork<ContentCategory> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> AddContentCategory(ContentCategoryCreateInfo contentCategoryCreateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var addRepository = unitOfWork.AddRepository;

        var newContentCategory = new ContentCategory
        {
            CategoryId = contentCategoryCreateInfo.BaseInfo.CategoryId,
            ContentId = contentCategoryCreateInfo.BaseInfo.ContentId
        };

        bool contentCategoryExists = (await findRepository.FindAsync(x => x.CategoryId == newContentCategory.CategoryId && x.ContentId == newContentCategory.ContentId)).Any();
        if (contentCategoryExists) return Result<bool>.Failure(Error.AlreadyExist());

        await addRepository.AddAsync(newContentCategory);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<ContentCategoryReadInfo>> GetContentCategoryById(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        ContentCategory? contentCategory = await findRepository.GetByIdAsync(id);
        if (contentCategory == null) return Result<ContentCategoryReadInfo>.Failure(Error.NotFound());

        return Result<ContentCategoryReadInfo>.Success(new ContentCategoryReadInfo(contentCategory.Id, new ContentCategoryBaseInfo(contentCategory.CategoryId, contentCategory.ContentId)));
    }

    public async Task<Result<PagedResponse<IEnumerable<ContentCategoryReadInfo>>>> GetContentCategories(ContentCategoryFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        Expression<Func<ContentCategory, bool>> filterExpression = contentCategory =>
            (filter.CategoryId == Guid.Empty || contentCategory.CategoryId == filter.CategoryId) &&
            (filter.ContentId == Guid.Empty || contentCategory.ContentId == filter.ContentId);

        var query = (await findRepository.FindAsync(filterExpression)).ToList();
        int count = query.Count();

        var contentCategories = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(contentCategory => new ContentCategoryReadInfo(contentCategory.Id, new ContentCategoryBaseInfo(contentCategory.CategoryId, contentCategory.ContentId)))
            .ToList();

        var pagedResponse = PagedResponse<IEnumerable<ContentCategoryReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, contentCategories);
        return Result<PagedResponse<IEnumerable<ContentCategoryReadInfo>>>.Success(pagedResponse);
    }

    public async Task<Result<bool>> UpdateContentCategory(Guid id, ContentCategoryUpdateInfo contentCategoryUpdateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        ContentCategory? contentCategory = await findRepository.GetByIdAsync(id);
        if (contentCategory == null) return Result<bool>.Failure(Error.NotFound());

        contentCategory.CategoryId = contentCategoryUpdateInfo.BaseInfo.CategoryId;
        contentCategory.ContentId = contentCategoryUpdateInfo.BaseInfo.ContentId;

        updateRepository.Update(contentCategory);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeleteContentCategory(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        ContentCategory? contentCategory = await findRepository.GetByIdAsync(id);
        if (contentCategory == null) return Result<bool>.Failure(Error.NotFound());

        deleteRepository.Delete(contentCategory);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }
}
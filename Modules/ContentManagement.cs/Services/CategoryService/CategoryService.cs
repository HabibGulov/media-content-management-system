using System.Linq.Expressions;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork<Category> unitOfWork;

    public CategoryService(IUnitOfWork<Category> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> AddCategory(CategoryCreateInfo categoryCreateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var addRepository = unitOfWork.AddRepository;

        bool categoryExists = (await findRepository.FindAsync(x => x.Name.ToLower() == categoryCreateInfo.BaseInfo.Name.ToLower())).Any();
        if (categoryExists) return Result<bool>.Failure(Error.AlreadyExist());

        var newCategory = new Category
        {
            Name = categoryCreateInfo.BaseInfo.Name,
            Description = categoryCreateInfo.BaseInfo.Description
        };

        await addRepository.AddAsync(newCategory);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<CategoryReadInfo>> GetCategoryById(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        Category? category = await findRepository.GetByIdAsync(id);
        if (category == null) return Result<CategoryReadInfo>.Failure(Error.NotFound());

        return Result<CategoryReadInfo>.Success(new CategoryReadInfo(id, new CategoryBaseInfo(category.Name, category.Description!)));
    }

    public async Task<Result<PagedResponse<IEnumerable<CategoryReadInfo>>>> GetCategories(CategoryFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        Expression<Func<Category, bool>> filterExpression = category =>
            (string.IsNullOrEmpty(filter.Name) || category.Name.Contains(filter.Name));

        var query = (await findRepository.FindAsync(filterExpression)).ToList();
        int count = query.Count();

        var categories = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(category => new CategoryReadInfo(category.Id, new CategoryBaseInfo(category.Name, category.Description!)))
            .ToList();

        var pagedResponse = PagedResponse<IEnumerable<CategoryReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, categories);
        return Result<PagedResponse<IEnumerable<CategoryReadInfo>>>.Success(pagedResponse);
    }

    public async Task<Result<bool>> UpdateCategory(Guid id, CategoryUpdateInfo categoryUpdateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        Category? category = await findRepository.GetByIdAsync(id);
        if (category == null) return Result<bool>.Failure(Error.NotFound());

        category.Name = categoryUpdateInfo.BaseInfo.Name;
        category.Description = categoryUpdateInfo.BaseInfo.Description;

        updateRepository.Update(category);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeleteCategory(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        Category? category = await findRepository.GetByIdAsync(id);
        if (category == null) return Result<bool>.Failure(Error.NotFound());

        deleteRepository.Delete(category);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }
}
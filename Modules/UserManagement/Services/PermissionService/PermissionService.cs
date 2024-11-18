using System.Linq.Expressions;

public class PermissionService : IPermissionService
{
    private readonly IUnitOfWork<Permission> unitOfWork;

    public PermissionService(IUnitOfWork<Permission> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> AddPermission(PermissionCreateInfo permissionCreateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var addRepository = unitOfWork.AddRepository;

        bool permissionExists = (await findRepository.FindAsync(x => x.Name.ToLower() == permissionCreateInfo.BaseInfo.Name.ToLower())).Any();
        if (permissionExists) return Result<bool>.Failure(Error.AlreadyExist());

        var newPermission = permissionCreateInfo.ToPermission();
        await addRepository.AddAsync(newPermission);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<PermissionReadInfo>> GetPermissionById(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        Permission? permission = await findRepository.GetByIdAsync(id);
        if (permission == null) return Result<PermissionReadInfo>.Failure(Error.NotFound());

        return Result<PermissionReadInfo>.Success(permission.ToReadInfo());
    }

    public async Task<Result<PagedResponse<IEnumerable<PermissionReadInfo>>>> GetPermissions(PermissionFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        Expression<Func<Permission, bool>> filterExpression = permission =>
            (string.IsNullOrEmpty(filter.Name) || permission.Name.Contains(filter.Name));

        var query = (await findRepository.FindAsync(filterExpression)).ToList();
        int count = query.Count();

        var permissions = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(permission => permission.ToReadInfo())
            .ToList();

        var pagedResponse = PagedResponse<IEnumerable<PermissionReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, permissions);
        return Result<PagedResponse<IEnumerable<PermissionReadInfo>>>.Success(pagedResponse);
    }

    public async Task<Result<bool>> UpdatePermission(Guid id, PermissionUpdateInfo permissionUpdateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        Permission? permission = await findRepository.GetByIdAsync(id);
        if (permission == null) return Result<bool>.Failure(Error.NotFound());

        var updatedPermission = permission.UpdatePermission(permissionUpdateInfo);
        updateRepository.Update(updatedPermission);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeletePermission(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        Permission? permission = await findRepository.GetByIdAsync(id);
        if (permission == null) return Result<bool>.Failure(Error.NotFound());

        deleteRepository.Delete(permission);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }
}
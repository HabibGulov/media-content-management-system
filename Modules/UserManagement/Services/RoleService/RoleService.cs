using System.Linq.Expressions;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork<Role> unitOfWork;

    public RoleService(IUnitOfWork<Role> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> AddRole(RoleCreateInfo roleCreateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var addRepository = unitOfWork.AddRepository;

        bool roleExists = (await findRepository.FindAsync(x => x.Name.ToLower() == roleCreateInfo.BaseInfo.Name.ToLower())).Any();
        if (roleExists) return Result<bool>.Failure(Error.AlreadyExist());

        var newRole = roleCreateInfo.ToRole(); 
        await addRepository.AddAsync(newRole);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<RoleReadInfo>> GetRoleById(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        Role? role = await findRepository.GetByIdAsync(id);
        if (role == null) return Result<RoleReadInfo>.Failure(Error.NotFound());

        return Result<RoleReadInfo>.Success(role.ToReadInfo());
    }

    public async Task<Result<bool>> UpdateRole(Guid id, RoleUpdateInfo roleUpdateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        Role? role = await findRepository.GetByIdAsync(id);
        if (role == null) return Result<bool>.Failure(Error.NotFound());

        role.UpdateRole(roleUpdateInfo); 
        updateRepository.Update(role);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeleteRole(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        Role? role = await findRepository.GetByIdAsync(id);
        if (role == null) return Result<bool>.Failure(Error.NotFound());

        deleteRepository.Delete(role);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<PagedResponse<IEnumerable<RoleReadInfo>>>> GetRoles(RoleFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        Expression<Func<Role, bool>> filterExpression = role =>
            string.IsNullOrEmpty(filter.Name) || role.Name.Contains(filter.Name);

        var query = (await findRepository.FindAsync(filterExpression)).ToList();
        int count = query.Count();

        var roles = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(role => role.ToReadInfo()) 
            .ToList();

        var pagedResponse = PagedResponse<IEnumerable<RoleReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, roles);
        return Result<PagedResponse<IEnumerable<RoleReadInfo>>>.Success(pagedResponse);
    }
}

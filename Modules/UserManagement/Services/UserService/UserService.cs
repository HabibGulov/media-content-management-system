using System.Linq.Expressions;

public class UserService : IUserService
{
    private readonly IUnitOfWork<User> unitOfWork;

    public UserService(IUnitOfWork<User> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> AddUser(UserCreateInfo info)
    {
        var findRepository = unitOfWork.FindRepository;
        var addRepository = unitOfWork.AddRepository;

        bool userExists = (await findRepository.FindAsync(x => x.Email.ToLower() == info.BaseInfo.Email.ToLower())).Any();
        if (userExists) return Result<bool>.Failure(Error.AlreadyExist());

        var newUser = info.ToUser();
        await addRepository.AddAsync(newUser);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<User>> Login(UserLogInInfo userLogInInfo)
    {
        var findRepository = unitOfWork.FindRepository;

        User? user = (await findRepository.FindAsync(x => x.Email.ToLower() == userLogInInfo.BaseInfo.Email.ToLower())).FirstOrDefault();
        if (user == null) return Result<User>.Failure(Error.NotFound());

        if (user.Password != userLogInInfo.Password)
            return Result<User>.Failure(Error.BadRequest());

        return Result<User>.Success(user);
    }

    public async Task<Result<UserReadInfo>> GetUserById(Guid id)
    {
        var findRepository = unitOfWork.FindRepository;
        User? user = await findRepository.GetByIdAsync(id);
        if (user == null) return Result<UserReadInfo>.Failure(Error.NotFound());

        return Result<UserReadInfo>.Success(user.ToReadInfo());
    }

    public async Task<Result<bool>> UpdateUser(Guid id, UserUpdateInfo userUpdateInfo)
    {
        var findRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        User? user = await findRepository.GetByIdAsync(id);
        if (user == null) return Result<bool>.Failure(Error.NotFound());

        var updatedUser = user.UpdateUser(userUpdateInfo);
        updateRepository.Update(updatedUser);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeleteUser(Guid id, string password)
    {
        var findRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        User? user = await findRepository.GetByIdAsync(id);
        if (user == null) return Result<bool>.Failure(Error.NotFound());
        if(user.Password!=password) return Result<bool>.Failure(Error.BadRequest());

        deleteRepository.Delete(user);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetUsers(UserFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        Expression<Func<User, bool>> filterExpression = user =>
            (string.IsNullOrEmpty(filter.UserName) || user.UserName.Contains(filter.UserName));

        var query = (await findRepository.FindAsync(filterExpression)).ToList();
        int count = query.Count();

        var users = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(user => user.ToReadInfo())
            .ToList();

        var pagedResponse = PagedResponse<IEnumerable<UserReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, users);
        return Result<PagedResponse<IEnumerable<UserReadInfo>>>.Success(pagedResponse);
    }
}
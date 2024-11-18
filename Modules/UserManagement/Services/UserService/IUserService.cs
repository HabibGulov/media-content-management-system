public interface IUserService
{
    Task<Result<bool>> AddUser(UserCreateInfo userCreateInfo);
    Task<Result<User>> Login(UserLogInInfo userLogInInfo);
    Task<Result<UserReadInfo>> GetUserById(Guid id);
    Task<Result<bool>> UpdateUser(Guid id, UserUpdateInfo userUpdateInfo);
    Task<Result<bool>> DeleteUser(Guid id, string password);
    Task<Result<PagedResponse<IEnumerable<UserReadInfo>>>> GetUsers(UserFilter filter);
}
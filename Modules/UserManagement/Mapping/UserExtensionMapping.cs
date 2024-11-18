public static class UserMappingExtensions
{
    public static UserReadInfo ToReadInfo(this User user)
    {
        return new UserReadInfo()
        {
            Id = user.Id,
            Role = user.Role.Name,
            BaseInfo = new UserBaseInfo()
            {
                RoleId = user.RoleId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            }
        };
    }

    public static User ToUser(this UserCreateInfo userCreateInfo)
    {
        return new User
        {
            RoleId = userCreateInfo.BaseInfo.RoleId,
            UserName = userCreateInfo.BaseInfo.UserName,
            Email = userCreateInfo.BaseInfo.Email,
            PhoneNumber = userCreateInfo.BaseInfo.PhoneNumber,
            Password = userCreateInfo.Password,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static User UpdateUser(this User user, UserUpdateInfo userUpdateInfo)
    {
        user.RoleId = userUpdateInfo.BaseInfo.RoleId;
        user.UserName = userUpdateInfo.BaseInfo.UserName;
        user.Email = userUpdateInfo.BaseInfo.Email;
        user.PhoneNumber = userUpdateInfo.BaseInfo.PhoneNumber;
        user.Password = userUpdateInfo.Password;
        user.UpdatedAt = DateTime.UtcNow;
        user.Version += 1;
        return user;
    }

    public static User DeleteUser(this User user)
    {
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        user.Version += 1;
        return user;
    }

    public static User FromUpdateToUser(this UserUpdateInfo updateInfo)
    {
        return new User()
        {
            RoleId = updateInfo.BaseInfo.RoleId,
            UserName = updateInfo.BaseInfo.UserName,
            Email = updateInfo.BaseInfo.Email,
            PhoneNumber = updateInfo.BaseInfo.PhoneNumber,
            Password = updateInfo.Password
        };
    }
}

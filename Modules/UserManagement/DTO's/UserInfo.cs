public readonly record struct UserBaseInfo(
    Guid RoleId,
    string UserName,
    string Email,
    string PhoneNumber
);

public readonly record struct UserLogInInfo(
    UserBaseInfo BaseInfo,
    string Password
    );

public readonly record struct UserCreateInfo(
    UserBaseInfo BaseInfo,
    string Password,
    string ConfirmPassword
);

public readonly record struct UserUpdateInfo(
    UserBaseInfo BaseInfo,
    string Password
);

public readonly record struct UserReadInfo(
    Guid Id,
    UserBaseInfo BaseInfo,
    string Role
);
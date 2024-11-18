using FluentValidation;

public class UserLogInInfoValidator : AbstractValidator<UserCreateInfo>
{
    public UserLogInInfoValidator()
    {
        RuleFor(x => x.BaseInfo.UserName)
            .NotEmpty().WithMessage("Имя пользователя не должно быть пустым.")
            .Length(3, 50).WithMessage("Имя пользователя должно быть от 3 до 50 символов.");

        RuleFor(x => x.BaseInfo.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Неверный формат Email.");

        RuleFor(x => x.BaseInfo.PhoneNumber)
            .NotEmpty().WithMessage("Номер телефона обязателен.")
            .Matches(@"^\+?\d+$").WithMessage("Неверный формат номера телефона.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов.")
            .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Пароль и подтверждение пароля должны совпадать.");
    }
}

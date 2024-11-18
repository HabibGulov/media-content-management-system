using FluentValidation;

public class ContentValidator : AbstractValidator<Content>
{
    public ContentValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Content title is required.")
            .Length(3, 200).WithMessage("Content title must be between 3 and 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Content description is required.")
            .Length(3, 1000).WithMessage("Content description must be between 3 and 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).When(x => !x.IsFree).WithMessage("Price must be greater than zero if the content is not free.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

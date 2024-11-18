using FluentValidation;

public class ContentCategoryValidator : AbstractValidator<ContentCategory>
{
    public ContentCategoryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.ContentId)
            .NotEmpty().WithMessage("Content ID is required.");
    }
}
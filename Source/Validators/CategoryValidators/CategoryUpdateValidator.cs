namespace OpenMovies.WebApi.Validators;

public sealed class CategoryUpdateValidator :
    AbstractValidator<CategoryUpdateRequest>, IValidator<CategoryUpdateRequest>
{
    public CategoryUpdateValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Category name must be at most 50 characters.");
    }
}
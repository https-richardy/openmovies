using FluentValidation;
using OpenMovies.Models;

namespace OpenMovies.Validators;

public class CategoryValidation : AbstractValidator<Category>
{
    public CategoryValidation()
    {
        RuleFor(category => category.Name)
            .NotEmpty().WithMessage("The category name cannot be empty.")
            .Must(ContainOnlyLetters).WithMessage("The category name must contain only letters.");
    }

    private bool ContainOnlyLetters(string name)
    {
        return !string.IsNullOrEmpty(name) && name.All(char.IsLetter);
    }
}
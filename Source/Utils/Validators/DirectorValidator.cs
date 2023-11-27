using FluentValidation;
using OpenMovies.Models;

namespace OpenMovies.Validators;

public class DirectorValidation : AbstractValidator<Director>
{
    public DirectorValidation()
    {
        RuleFor(director => director.FirstName)
            .NotEmpty().WithMessage("The director's first name cannot be empty.")
            .Length(2, 50).WithMessage("The director's first name must be between 2 and 50 characters.");

        RuleFor(director => director.LastName)
            .NotEmpty().WithMessage("The director's last name cannot be empty.")
            .Length(2, 50).WithMessage("The director's last name must be between 2 and 50 characters.");
    }
}

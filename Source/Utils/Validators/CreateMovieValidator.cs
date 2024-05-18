namespace OpenMovies.WebApi.Validators;

public class MovieValidation : AbstractValidator<Movie>
{
    public MovieValidation()
    {
        RuleFor(movie => movie.Title)
            .NotEmpty().WithMessage("The title cannot be empty.")
            .Length(3, 100).WithMessage("The title must be between 3 and 100 characters.");

        RuleFor(movie => movie.ReleaseYear)
            .NotEmpty().WithMessage("The release date cannot be empty.");

        RuleFor(movie => movie.Synopsis)
            .NotEmpty().WithMessage("The synopsis cannot be empty.")
            .Length(60, 1000).WithMessage("The synopsis must be between 60 and 1000 characters.");

        RuleFor(movie => movie.Category.Id)
            .GreaterThan(0).WithMessage("The category ID must be greater than zero.");
    }
}
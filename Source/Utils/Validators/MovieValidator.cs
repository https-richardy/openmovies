namespace OpenMovies.WebApi.Validators;

public class MovieValidation : AbstractValidator<Movie>, IValidator<Movie>
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

        RuleFor(movie => movie.ReleaseYear)
            .NotEmpty().WithMessage("The release year cannot be empty.")
            .GreaterThan(0).WithMessage("The release year must be greater than 0.");

        RuleFor(movie => movie.Category)
            .NotNull().WithMessage("The category cannot be null.")
            .IsInEnum();

        RuleFor(movie => movie.Duration)
            .NotNull().WithMessage("The duration cannot be null");
    }
}
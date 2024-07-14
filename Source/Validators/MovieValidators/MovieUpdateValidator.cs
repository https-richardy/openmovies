namespace OpenMovies.WebApi.Validators;

public sealed class MovieUpdateValidator :
    AbstractValidator<MovieUpdateRequest>, IValidator<MovieUpdateRequest>
{
    public MovieUpdateValidator()
    {
        RuleFor(movie => movie.Title)
            .NotEmpty().WithMessage("Movie title is required.")
            .MaximumLength(120).WithMessage("Movie title must be at most 120 characters.");
    
        RuleFor(movie => movie.Synopsis)
            .NotEmpty().WithMessage("Movie synopsis is required.")
            .MaximumLength(1000).WithMessage("Movie synopsis must be at most 1000 characters.");

        RuleFor(movie => movie.VideoSource)
            .NotEmpty().WithMessage("Movie video source (CDN link) is required.");

        RuleFor(movie => movie.ReleaseYear)
            .NotEmpty().WithMessage("Movie release year is required.")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Movie release year must be in the past.");

        RuleFor(movie => movie.DurationInMinutes)
            .NotEmpty().WithMessage("Movie duration in minutes is required.")
            .GreaterThan(0).WithMessage("Movie duration in minutes must be greater than 0.");

        RuleFor(movie => movie.CategoryId)
            .NotEmpty().WithMessage("Movie category is required.")
            .GreaterThan(0).WithMessage("Movie category must be greater than 0.");

        RuleFor(movie => movie.MovieId)
            .NotEmpty().WithMessage("Movie id is required.")
            .GreaterThan(0).WithMessage("Movie id must be greater than 0.");
    }
}
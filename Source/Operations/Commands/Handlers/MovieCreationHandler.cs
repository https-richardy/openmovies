namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class MovieCreationHandler : IRequestHandler<MovieCreationRequest, MovieCreationResponse>
{
    private readonly IMovieService _movieService;
    private readonly IValidator<Movie> _validator;

    public MovieCreationHandler(IMovieService movieService, IValidator<Movie> validator)
    {
        _movieService = movieService;
        _validator = validator;
    }

    public async Task<MovieCreationResponse> Handle(MovieCreationRequest request, CancellationToken cancellationToken)
    {
        var movie = TinyMapper.Map<Movie>(request);
        var validationResult = await _validator.ValidateAsync(movie);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _movieService.CreateMovieAsync(movie);

        return MovieCreationResponse.SuccessResponse();
    }
}
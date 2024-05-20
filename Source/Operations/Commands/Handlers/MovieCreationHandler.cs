namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class MovieCreationHandler : IRequestHandler<MovieCreationRequest, MovieCreationResponse>
{
    private readonly IMovieService _movieService;
    private readonly IValidator<MovieCreationRequest> _validator;

    public MovieCreationHandler(IMovieService movieService, IValidator<MovieCreationRequest> validator)
    {
        _movieService = movieService;
        _validator = validator;
    }

    public async Task<MovieCreationResponse> Handle(MovieCreationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var movie = TinyMapper.Map<Movie>(request);
        await _movieService.CreateMovieAsync(movie);

        return MovieCreationResponse.SuccessResponse();
    }
}
namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class MovieDeletionHandler : IRequestHandler<MovieDeletionRequest, MovieDeletionResponse>
{
    private readonly IMovieService _movieService;

    public MovieDeletionHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<MovieDeletionResponse> Handle(MovieDeletionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _movieService.DeleteMovieAsync(request.MovieId);
            return MovieDeletionResponse.SuccessResponse();
        }
        catch (InvalidOperationException exception)
        {
            return MovieDeletionResponse.FailureResponse(exception.Message);
        }
    }
}
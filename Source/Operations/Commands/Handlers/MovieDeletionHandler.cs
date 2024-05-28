namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class MovieDeletionHandler : IRequestHandler<MovieDeletionRequest, OperationResult>
{
    private readonly IMovieService _movieService;

    public MovieDeletionHandler(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<OperationResult> Handle(MovieDeletionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _movieService.DeleteMovieAsync(request.MovieId);
            return OperationResult.SuccessResponse("Movie deleted successfully.");
        }
        catch (InvalidOperationException exception)
        {
            return OperationResult.FailureResponse(exception.Message);
        }
    }
}
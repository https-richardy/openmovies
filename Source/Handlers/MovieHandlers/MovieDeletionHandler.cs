namespace OpenMovies.WebApi.Handlers;

public sealed class MovieDeletionHandler(
    IMovieRepository movieRepository,
    ILogger<MovieDeletionHandler> logger
) : IRequestHandler<MovieDeletionRequest, Response>
{
    public async Task<Response> Handle(
        MovieDeletionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var movie = await movieRepository.GetByIdAsync(request.MovieId);
            if (movie is null)
                return new Response(
                    statusCode: StatusCodes.Status404NotFound,
                    message: "movie not found"
                );

            await movieRepository.DeleteAsync(movie);
            logger.LogInformation("Movie with id `{MovieId}` deleted successfully.", request.MovieId);

            return new Response(
                statusCode: StatusCodes.Status200OK,
                message: "movie deleted successfully"
            );
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to delete movie.");
            return new Response(
                statusCode: StatusCodes.Status500InternalServerError,
                message: exception.Message
            );
        }
    }
}
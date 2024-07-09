namespace OpenMovies.WebApi.Handlers;

public sealed class MovieDetailsHandler(
    IMovieRepository movieRepository,
    ILogger<MovieDetailsHandler> logger
) : IRequestHandler<MovieDetailsRequest, Response<Movie>>
{
    public async Task<Response<Movie>> Handle(
        MovieDetailsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Retrieving movie details for movie with id `{MovieId}`.", request.MovieId);

            var movie = await movieRepository.GetByIdAsync(request.MovieId);
            if (movie is null)
                return new Response<Movie>(
                    data: null,
                    statusCode: StatusCodes.Status404NotFound,
                    message: "Movie not found."
                );

            logger.LogInformation("Movie details retrieved successfully.");

            return new Response<Movie>(
                data: movie,
                statusCode: StatusCodes.Status200OK,
                message: "Movie details retrieved successfully."
            );
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to retrieve movie details.");
            return new Response<Movie>(
                data: null,
                statusCode: StatusCodes.Status500InternalServerError,
                message: "Failed to retrieve movie details."
            );
        }
    }
}
namespace OpenMovies.WebApi.Handlers;

public sealed class GetMoviesHandler(
    IMovieRepository movieRepository,
    ILogger<GetMoviesHandler> logger,
    IHttpContextAccessor contextAccessor
) :
    IRequestHandler<GetMoviesRequest, Response<PaginationHelper<Movie>>>
{
    #pragma warning disable CS8604
    public async Task<Response<PaginationHelper<Movie>>> Handle(
        GetMoviesRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            Expression<Func<Movie, bool>> predicate = movie =>
                (string.IsNullOrEmpty(request.Title) || movie.Title.Contains(request.Title.ToLower())) &&
                (!request.Year.HasValue || movie.ReleaseYear == request.Year);

            var movies = await movieRepository.PagedAsync(predicate, request.Page, request.PageSize);
            var paginationHelper = new PaginationHelper<Movie>(
                data: movies,
                pageNumber: request.Page,
                pageSize: request.PageSize,
                httpContext: contextAccessor.HttpContext
            );

            return new Response<PaginationHelper<Movie>>(
                data: paginationHelper,
                statusCode: StatusCodes.Status200OK,
                message: "movies retrieved successfully"
            );
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to retrieve paginated movies.");
            return new Response<PaginationHelper<Movie>>(
                data: null,
                statusCode: StatusCodes.Status500InternalServerError,
                message: "failed to retrieve paginated movies"
            );
        }
    }
}
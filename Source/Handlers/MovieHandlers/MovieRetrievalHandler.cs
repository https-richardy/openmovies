namespace OpenMovies.WebApi.Handlers;

public sealed class MovieRetrievalHandler(
    IMovieRepository movieRepository,
    ILogger<MovieRetrievalHandler> logger,
    IHttpContextAccessor contextAccessor
) :
    IRequestHandler<MovieRetrievalRequest, Response<PaginationHelper<Movie>>>
{
    #pragma warning disable CS8604
    public async Task<Response<PaginationHelper<Movie>>> Handle(
        MovieRetrievalRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            Expression<Func<Movie, bool>> predicate = movie =>
                (string.IsNullOrEmpty(request.Title) || movie.Title.ToLower().Contains(request.Title)) &&
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
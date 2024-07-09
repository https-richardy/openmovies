namespace OpenMovies.WebApi.Handlers;

public sealed class MovieUpdateHandler(
    IMovieRepository movieRepository,
    ICategoryRepository categoryRepository,
    IFileUploadService fileUploadService,
    IValidator<MovieUpdateRequest> validator,
    ILogger<MovieUpdateHandler> logger
)
{
    public async Task<Response> Handle(
        MovieUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var movie = await movieRepository.GetByIdAsync(request.MovieId);
        if (movie is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "movie not found"
            );

        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "category not found"
            );

        movie = TinyMapper.Map<Movie>(request);

        movie.Category = category;
        movie.ImageUrl = await fileUploadService.UploadFileAsync(request.Image);

        await movieRepository.UpdateAsync(movie);
        logger.LogInformation("Movie {MovieTitle} updated successfully.", movie.Title);

        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "movie updated successfully"
        );
   }
}
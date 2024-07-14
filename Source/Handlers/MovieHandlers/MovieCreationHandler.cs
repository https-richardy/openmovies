namespace OpenMovies.WebApi.Handlers;

public sealed class MovieCreationHandler(
    IMovieRepository movieRepository,
    ICategoryRepository categoryRepository,
    IFileUploadService fileUploadService,
    IValidator<MovieCreationRequest> validator,
    ILogger<MovieCreationHandler> logger
) :
    IRequestHandler<MovieCreationRequest, Response>
{
    public async Task<Response> Handle(
        MovieCreationRequest request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "category not found"
            );

        var movie = TinyMapper.Map<Movie>(request);
        var imagePath = await fileUploadService.UploadFileAsync(request.Image);

        movie.ImageUrl = imagePath;
        movie.Category = category;

        await movieRepository.SaveAsync(movie);

        logger.LogInformation("Movie {MovieTitle} created successfully.", movie.Title);

        return new Response(
            statusCode: StatusCodes.Status201Created,
            message: "movie created successfully"
        );
    }
}
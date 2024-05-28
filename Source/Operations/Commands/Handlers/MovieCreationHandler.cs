namespace OpenMovies.WebApi.Operations.Commands.Handlers;

public sealed class MovieCreationHandler : IRequestHandler<MovieCreationRequest, OperationResult>
{
    private readonly IMovieService _movieService;
    private readonly IValidator<Movie> _validator;
    private readonly IFileUploadService _fileUploadService;

    public MovieCreationHandler(IMovieService movieService, IValidator<Movie> validator, IFileUploadService fileUploadService)
    {
        _movieService = movieService;
        _validator = validator;
        _fileUploadService = fileUploadService;
    }

    public async Task<OperationResult> Handle(MovieCreationRequest request, CancellationToken cancellationToken)
    {
        var movie = TinyMapper.Map<Movie>(request);
        var validationResult = await _validator.ValidateAsync(movie);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (request.Cover != null)
        {
            var imagePath = await _fileUploadService.UploadFileAsync(request.Cover);
            movie.ImagePath = imagePath;
        }

        await _movieService.CreateMovieAsync(movie);

        return OperationResult.SuccessResponse("Movie created successfully.");
    }
}
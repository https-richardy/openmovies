namespace OpenMovies.TestingSuite.HandlersTestSuite.MovieHandlers;

public sealed class MovieCreationHandlerTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IFileUploadService> _fileUploadServiceMock;
    private readonly Mock<IValidator<MovieCreationRequest>> _validatorMock;
    private readonly Mock<ILogger<MovieCreationHandler>> _loggerMock;
    private readonly Mock<IFormFile> _formFileMock;

    private readonly IFixture _fixture;
    private readonly IServiceCollection _serviceCollection;
    private readonly IRequestHandler<MovieCreationRequest, Response> _handler;

    public MovieCreationHandlerTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _fileUploadServiceMock = new Mock<IFileUploadService>();

        _validatorMock = new Mock<IValidator<MovieCreationRequest>>();
        _loggerMock = new Mock<ILogger<MovieCreationHandler>>();
        _formFileMock = new Mock<IFormFile>();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _serviceCollection = new ServiceCollection();
        _serviceCollection.AddMapping(); // Add mapping settings with TinyMapper

        _handler = new MovieCreationHandler(
            movieRepository: _movieRepositoryMock.Object,
            categoryRepository: _categoryRepositoryMock.Object,
            fileUploadService: _fileUploadServiceMock.Object,
            validator: _validatorMock.Object,
            logger: _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid movie creation request, should create movie successfully")]
    public async Task GivenValidMovieCreationRequest_ShouldCreateMovieSuccessfully()
    {
        _formFileMock.SetupGet(form => form.FileName)
            .Returns("image.jpg");

        var uploadedImagePath = "uploads/image/path.jpg";
        var category = new Category { Id = 1, Name = "Test Category" };
        var request = new MovieCreationRequest
        {
            Title = "Test Movie",
            Synopsis = "Test Synopsis",
            Image = _formFileMock.Object,
            CategoryId = 1
        };

        var validationResult = new ValidationResult();

        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);


        _categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync(category);

        _fileUploadServiceMock.Setup(service => service.UploadFileAsync(request.Image))
            .ReturnsAsync(uploadedImagePath);

        var result = await _handler.Handle(request, default);

        _validatorMock.Verify(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
        _categoryRepositoryMock.Verify(repo => repo.GetByIdAsync(request.CategoryId), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(request.Image), Times.Once);
        _movieRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Movie>()), Times.Once);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        Assert.Equal("movie created successfully", result.Message);
    }

    [Fact(DisplayName = "Given invalid movie creation request, should return validation error")]
    public async Task GivenInvalidMovieCreationRequest_ShouldReturnValidationError()
    {
        var request = new MovieCreationRequest();
        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title is required."),
            new ValidationFailure("Synopsis", "Synopsis is required."),
            new ValidationFailure("CategoryId", "Category is required.")
        };

        var validationResult = new ValidationResult { Errors = validationErrors };
        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, default));
    }

    [Fact(DisplayName = "Given non-existing category id, should return not found")]
    public async Task GivenNonExistingCategoryId_ShouldReturnNotFound()
    {
        var request = new MovieCreationRequest { CategoryId = 999 };
        var validationResult = new ValidationResult();

        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        #pragma warning disable CS8600 // disable CS8600 because it needs to be null since in this scenario no category is found.
        _categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync((Category)null);

        var result = await _handler.Handle(request, default);
        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(request.CategoryId), Times.Once);

        Assert.NotNull(result);

        Assert.False(result.IsSuccess);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        Assert.Equal("category not found", result.Message);
    }
}
namespace OpenMovies.TestingSuite.HandlersTestSuite.MovieHandlers;

public sealed class MovieUpdateHandlerTest
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IFileUploadService> _fileUploadServiceMock;
    private readonly Mock<IValidator<MovieUpdateRequest>> _validatorMock;
    private readonly Mock<ILogger<MovieUpdateHandler>> _loggerMock;
    private readonly MovieUpdateHandler _handler;

    public MovieUpdateHandlerTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _fileUploadServiceMock = new Mock<IFileUploadService>();
        _validatorMock = new Mock<IValidator<MovieUpdateRequest>>();
        _loggerMock = new Mock<ILogger<MovieUpdateHandler>>();

        _handler = new MovieUpdateHandler(
            _movieRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _fileUploadServiceMock.Object,
            _validatorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given an invalid request, should throw ValidationException")]
    public async Task GivenInvalidRequest_ShouldThrowValidationException()
    {
        var request = new MovieUpdateRequest();
        var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Title", "Title is required")
            };

        var validationResult = new ValidationResult(validationFailures);

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));

        _movieRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(It.IsAny<IFormFile>()), Times.Never);
    }

    [Fact(DisplayName = "Given a non-existent movie, should return 404 Not Found response")]
    public async Task GivenNonExistentMovie_ShouldReturnNotFoundResponse()
    {
        var request = new MovieUpdateRequest { MovieId = 1 };
        var validationResult = new ValidationResult();

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        #pragma warning disable CS8600 // disable CS8600 because it needs to be null since in this scenario no movie is found.
        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.MovieId))
            .ReturnsAsync((Movie)null); 

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("movie not found", response.Message);
    }

    [Fact(DisplayName = "Given a non-existent category, should return 404 Not Found response")]
    public async Task GivenNonExistentCategory_ShouldReturnNotFoundResponse()
    {
        var request = new MovieUpdateRequest { MovieId = 1, CategoryId = 2 };
        var validationResult = new ValidationResult();

        var movie = new Movie { Id = 1, Title = "Test Movie" };

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.MovieId))
            .ReturnsAsync(movie);

        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync((Category)null);

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("category not found", response.Message);
    }

    [Fact(DisplayName = "Given a valid request, should return 200 OK response")]
    public async Task GivenValidRequest_ShouldReturnOkResponse()
    {
        var formFile = new Mock<IFormFile>();
        var request = new MovieUpdateRequest { MovieId = 1, CategoryId = 2, Image = formFile.Object };

        var validationResult = new ValidationResult();

        var movie = new Movie { Id = 1, Title = "Test Movie" };
        var category = new Category { Id = 2, Name = "Test Category" };

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _movieRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.MovieId))
            .ReturnsAsync(movie);

        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync(category);

        _fileUploadServiceMock
            .Setup(service => service.UploadFileAsync(request.Image))
            .ReturnsAsync("uploaded_image_url");

        _movieRepositoryMock
            .Setup(repository => repository.UpdateAsync(It.IsAny<Movie>()))
            .ReturnsAsync(new OperationResult { IsSuccess = true });

        var response = await _handler.Handle(request, CancellationToken.None);

        _movieRepositoryMock.Verify(repository => repository.GetByIdAsync(request.MovieId), Times.Once);
        _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(request.CategoryId), Times.Once);

        _validatorMock.Verify(validator => validator.ValidateAsync(request, CancellationToken.None), Times.Once);
        _fileUploadServiceMock.Verify(service => service.UploadFileAsync(request.Image), Times.Once);
        _movieRepositoryMock.Verify(repository => repository.UpdateAsync(It.IsAny<Movie>()), Times.Once);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("movie updated successfully", response.Message);
    }
}
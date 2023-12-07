namespace OpenMovies.Controllers.Tests;

public class MovieControllerTests
{
    private readonly MovieController _controller;
    private readonly Fixture _fixture;
    private readonly Mock<IMovieService> _movieService;
    private readonly Mock<ICategoryService> _categoryService;
    private readonly Mock<IDirectorService> _directorService;
    private readonly Mock<IWebHostEnvironment> _hostEnvironment;

    public MovieControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _movieService = new Mock<IMovieService>();
        _categoryService = new Mock<ICategoryService>();
        _directorService = new Mock<IDirectorService>();
        _hostEnvironment = new Mock<IWebHostEnvironment>();

        var httpContext = new DefaultHttpContext();

        _controller = new MovieController(
            _movieService.Object,
            _categoryService.Object,
            _directorService.Object,
            _hostEnvironment.Object);

        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetAll_ReturnsPagedData()
    {
        var pageNumber = 1;
        var pageSize = 10;

        var movies = _fixture.CreateMany<Movie>(20).ToList();
        _movieService.Setup(service => service.GetAllMovies())
            .ReturnsAsync(movies);

        var result = await _controller.GetAll(pageNumber, pageSize);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var pagination = Assert.IsType<Pagination<Movie>>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(10, pagination.Results.Count);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResultWithData()
    {
        var movieId = 1;
        var expectedMovie = _fixture.Create<Movie>();

        _movieService.Setup(service => service.GetMovieById(movieId))
            .ReturnsAsync(expectedMovie);

        var result = await _controller.GetById(movieId);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var actualMovie = Assert.IsType<Movie>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(expectedMovie, actualMovie);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundResult()
    {
        var invalidMovieId = -1;
        _movieService.Setup(service => service.GetMovieById(invalidMovieId))
            .ThrowsAsync(new InvalidOperationException("Movie not found"));

        var result = await _controller.GetById(invalidMovieId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResult()
    {
        var movieDTO = new Mock<MovieDTO>().Object;

        var director = _fixture.Create<Director>();
        var category = _fixture.Create<Category>();

        _directorService.Setup(service => service.GetDirectorById(movieDTO.DirectorId))
            .ReturnsAsync(director);

        _categoryService.Setup(service => service.GetCategoryById(movieDTO.CategoryId))
            .ReturnsAsync(category);

        var result = await _controller.Create(movieDTO);
        var actionResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(201, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidData_ValidationError_ReturnsBadRequestResult()
    {
        var movieDTO = new Mock<MovieDTO>().Object;

        var validationErrors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message") };

        _directorService.Setup(service => service.GetDirectorById(movieDTO.DirectorId))
            .ReturnsAsync(_fixture.Create<Director>());

        _categoryService.Setup(service => service.GetCategoryById(movieDTO.CategoryId))
            .ReturnsAsync(_fixture.Create<Category>());

        _movieService.Setup(service => service.CreateMovie(It.IsAny<Movie>()))
            .ThrowsAsync(new ValidationException(validationErrors));

        var result = await _controller.Create(movieDTO);
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_WithInvalidData_MovieAlreadyExists_ReturnsNotFoundResult()
    {
        var movieDTO = new Mock<MovieDTO>().Object;
        var existingMovie = _fixture.Create<Movie>();

        _directorService.Setup(service => service.GetDirectorById(movieDTO.DirectorId))
            .ReturnsAsync(_fixture.Create<Director>());

        _categoryService.Setup(service => service.GetCategoryById(movieDTO.CategoryId))
            .ReturnsAsync(_fixture.Create<Category>());

        _movieService.Setup(service => service.CreateMovie(It.IsAny<Movie>()))
            .ThrowsAsync(new InvalidOperationException("Movie already exists"));

        var result = await _controller.Create(movieDTO);
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_WithValidDataAndExistingId_ReturnsNoContent()
    {
        var id = 1;

        var movieDTO = new Mock<MovieDTO>().Object;
        var existingMovie = _fixture.Create<Movie>();

        _movieService.Setup(service => service.GetMovieById(id))
            .ReturnsAsync(existingMovie);

        var result = await _controller.Update(id, movieDTO);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        var id = 1;
        var validationErrors = new List<ValidationFailure> { new ValidationFailure("Property", "Error message") };

        var movieDTO = new Mock<MovieDTO>().Object;
        var existingMovie = _fixture.Create<Movie>();

        _movieService.Setup(service => service.GetMovieById(id))
            .ReturnsAsync(existingMovie);

        _movieService.Setup(service => service.UpdateMovie(existingMovie))
            .ThrowsAsync(new ValidationException(validationErrors));

        var result = await _controller.Update(id, movieDTO);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_WithNonExistentId_ReturnsNotFoundResult()
    {
        var nonExistentMovieId = -1;
        var updatedMovieDto = new Mock<MovieDTO>().Object;

        _movieService.Setup(service => service.GetMovieById(nonExistentMovieId))
            .ThrowsAsync(new InvalidOperationException("Movie not found"));

        var result = await _controller.Update(nonExistentMovieId, updatedMovieDto);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WithValidId_ReturnsNoContentResult()
    {
        var existingMovieId = 1;

        var result = await _controller.Delete(existingMovieId);
        var actionResult = Assert.IsType<NoContentResult>(result);

        Assert.Equal(204, actionResult.StatusCode);
        _movieService.Verify(service => service.DeleteMovie(existingMovieId), Times.Once);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ReturnsNotFoundResult()
    {
        var nonExistentMovieId = -1;

        _movieService.Setup(service => service.DeleteMovie(nonExistentMovieId))
            .ThrowsAsync(new InvalidOperationException("Movie not found"));

        var result = await _controller.Delete(nonExistentMovieId);
        var actionResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, actionResult.StatusCode);
        _movieService.Verify(service => service.DeleteMovie(nonExistentMovieId), Times.Once);
    }

    [Fact]
    public async Task Search_WithValidParameters_ReturnsOkResult()
    {
        var searchName = "Test Movie";
        var searchReleaseYear = 2022;

        var searchCategoryId = 1;
        var pageNumber = 1;
        var pageSize = 10;

        var movies = _fixture.CreateMany<Movie>(20).ToList();
        _movieService.Setup(service => service.SearchMovies(searchName, searchReleaseYear, searchCategoryId))
            .ReturnsAsync(movies);

        var result = await _controller.Search(searchName, searchReleaseYear, searchCategoryId, pageNumber, pageSize);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var pagination = Assert.IsType<Pagination<Movie>>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(20, pagination.Count);

        _movieService.Verify(service => service.SearchMovies(searchName, searchReleaseYear, searchCategoryId), Times.Once);
    }
}
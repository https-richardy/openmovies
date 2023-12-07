namespace OpenMovies.Controllers.Tests;

public class DirectorControllerTests
{
    private readonly DirectorController _controller;
    private readonly Fixture _fixture;
    private readonly Mock<IDirectorService> _directorService;

    public DirectorControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _directorService = new Mock<IDirectorService>();

        var httpContext = new DefaultHttpContext();

        _controller = new DirectorController(_directorService.Object);
        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        var directors = _fixture.CreateMany<Director>(10).ToList();
        _directorService.Setup(service => service.GetAllDirectors()).ReturnsAsync(directors);

        var result = await _controller.GetAll();

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retrievedDirectors = Assert.IsType<List<Director>>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(10, retrievedDirectors.Count);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        var directorId = 1;
        var director = _fixture.Create<Director>();
        _directorService.Setup(service => service.GetDirectorById(directorId)).ReturnsAsync(director);

        var result = await _controller.GetById(directorId);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retrievedDirector = Assert.IsType<Director>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(director.Id, retrievedDirector.Id);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundResult()
    {
        var invalidDirectorId = 99;
        _directorService.Setup(service => service.GetDirectorById(invalidDirectorId)).ThrowsAsync(new InvalidOperationException("Director not found"));

        var result = await _controller.GetById(invalidDirectorId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResult()
    {
        var directorDTO = _fixture.Create<DirectorDTO>();
        _directorService.Setup(service => service.CreateDirector(It.IsAny<Director>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(directorDTO);
        var actionResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(201, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }

    [Fact]
    public async Task Create_WithInvalidData_ReturnsBadRequestResult()
    {
        var invalidDirectorDTO = new DirectorDTO();
        _controller.ModelState.AddModelError("FirstName", "The FirstName field is required.");

        _directorService.Setup(service => service.CreateDirector(It.IsAny<Director>()))
            .ThrowsAsync(new ValidationException("Validation failed", new List<ValidationFailure>
            {
                new ValidationFailure("FirstName", "The FirstName field is required.")
            }));

        var result = await _controller.Create(invalidDirectorDTO);
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }

    [Fact]
    public async Task Create_WithDuplicateDirector_ReturnsConflictResult()
    {
        var directorDTO = _fixture.Create<DirectorDTO>();
        _directorService.Setup(service => service.CreateDirector(It.IsAny<Director>())).ThrowsAsync(new InvalidOperationException("Director already exists"));

        var result = await _controller.Create(directorDTO);
        var actionResult = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal(409, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }
}
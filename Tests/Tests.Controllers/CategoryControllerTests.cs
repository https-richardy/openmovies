namespace OpenMovies.Controllers.Tests;

public class CategoryControllerTests
{
    private readonly CategoryController _controller;
    private readonly Fixture _fixture;
    private readonly Mock<ICategoryService> _categoryService;

    public CategoryControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _categoryService = new Mock<ICategoryService>();

        var httpContext = new DefaultHttpContext();

        _controller = new CategoryController(_categoryService.Object);
        _controller.ControllerContext.HttpContext = httpContext;
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        var categories = _fixture.CreateMany<Category>(10).ToList();
        _categoryService.Setup(service => service.GetAllCategories()).ReturnsAsync(categories);

        var result = await _controller.GetAll();

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retrievedCategories = Assert.IsType<List<Category>>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(10, retrievedCategories.Count);
    }

    [Fact]
    public async Task GetById_WithValidId_ReturnsOkResult()
    {
        var categoryId = 1;
        var category = _fixture.Create<Category>();
        _categoryService.Setup(service => service.GetCategoryById(categoryId)).ReturnsAsync(category);

        var result = await _controller.GetById(categoryId);

        var actionResult = Assert.IsType<OkObjectResult>(result);
        var retrievedCategory = Assert.IsType<Category>(actionResult.Value);

        Assert.Equal(200, actionResult.StatusCode);
        Assert.Equal(category.Id, retrievedCategory.Id);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFoundResult()
    {
        var invalidCategoryId = 99;
        _categoryService.Setup(service => service.GetCategoryById(invalidCategoryId)).ThrowsAsync(new InvalidOperationException("Category not found"));

        var result = await _controller.GetById(invalidCategoryId);

        var actionResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, actionResult.StatusCode);
    }

    [Fact]
    public async Task Create_WithValidData_ReturnsCreatedResult()
    {
        var categoryDTO = _fixture.Create<CategoryDTO>();
        _categoryService.Setup(service => service.CreateCategory(It.IsAny<Category>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(categoryDTO);
        var actionResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(201, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }

    [Fact]
    public async Task Create_WithInvalidData_ReturnsBadRequestResult()
    {
        var invalidCategoryDTO = new CategoryDTO();
        _controller.ModelState.AddModelError("Name", "The Name field is required.");

        _categoryService.Setup(service => service.CreateCategory(It.IsAny<Category>()))
            .ThrowsAsync(new ValidationException("Validation failed", new List<ValidationFailure>
            {
                new ValidationFailure("Name", "The Name field is required.")
            }));

        var result = await _controller.Create(invalidCategoryDTO);
        var actionResult = Assert.IsType<BadRequestObjectResult>(result);

        Assert.Equal(400, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }

    [Fact]
    public async Task Create_WithDuplicateCategory_ReturnsConflictResult()
    {
        var categoryDTO = _fixture.Create<CategoryDTO>();
        _categoryService.Setup(service => service.CreateCategory(It.IsAny<Category>())).ThrowsAsync(new InvalidOperationException("Category already exists"));

        var result = await _controller.Create(categoryDTO);
        var actionResult = Assert.IsType<ConflictObjectResult>(result);

        Assert.Equal(409, actionResult.StatusCode);
        Assert.NotNull(actionResult.Value);
    }
}

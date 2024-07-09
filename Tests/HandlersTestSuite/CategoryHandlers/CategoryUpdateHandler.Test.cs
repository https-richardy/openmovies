namespace OpenMovies.TestingSuite.HandlersTestSuite.CategoryHandlers;

public sealed class CategoryUpdateHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IValidator<CategoryUpdateRequest>> _validatorMock;
    private readonly Mock<ILogger<CategoryUpdateHandler>> _loggerMock;
    private readonly CategoryUpdateHandler _handler;

    public CategoryUpdateHandlerTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _validatorMock = new Mock<IValidator<CategoryUpdateRequest>>();
        _loggerMock = new Mock<ILogger<CategoryUpdateHandler>>();

        _handler = new CategoryUpdateHandler(
            _categoryRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given an invalid request, should throw ValidationException")]
    public async Task GivenInvalidRequest_ShouldThrowValidationException()
    {
        var request = new CategoryUpdateRequest { CategoryId = 1, Name = "" };
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure(nameof(CategoryUpdateRequest.Name), "Name is required")
        });

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));

        Assert.Contains(validationResult.Errors, error => error.ErrorMessage == "Name is required");
    }

    [Fact(DisplayName = "Given category does not exist, should return 404 NotFound")]
    public async Task GivenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var request = new CategoryUpdateRequest { CategoryId = 1, Name = "New Category Name" };

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        #pragma warning disable CS8600
        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync((Category)null); // disabling warning because in this case the category needs to be null.

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal("category not found", response.Message);
    }

    [Fact(DisplayName = "Given a valid request, should update category and return 200 OK")]
    public async Task GivenValidRequest_ShouldUpdateCategoryAndReturnOk()
    {
        var category = new Category { Id = 1, Name = "Old Category Name" };
        var request = new CategoryUpdateRequest { CategoryId = 1, Name = "New Category Name" };

        _validatorMock
            .Setup(validator => validator.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        _categoryRepositoryMock
            .Setup(repository => repository.GetByIdAsync(request.CategoryId))
            .ReturnsAsync(category);

        _categoryRepositoryMock
            .Setup(repository => repository.SaveAsync(category))
            .ReturnsAsync(new OperationResult(isSuccess: true, message: "category updated successfully"));

        var response = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal("category updated successfully", response.Message);
    }
}
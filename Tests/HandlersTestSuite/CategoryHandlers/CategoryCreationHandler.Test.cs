namespace OpenMovies.TestingSuite.HandlersTestSuite.CategoryHandlers;

public sealed class CategoryCreationHandlerTest
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IValidator<CategoryCreationRequest>> _validatorMock;
    private readonly Mock<ILogger<CategoryCreationHandler>> _loggerMock;
    private readonly IRequestHandler<CategoryCreationRequest, Response> _handler;

    public CategoryCreationHandlerTest()
    {
        #region  Mocking

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _validatorMock = new Mock<IValidator<CategoryCreationRequest>>();
        _loggerMock = new Mock<ILogger<CategoryCreationHandler>>();

        #endregion

        _handler = new CategoryCreationHandler(
            categoryRepository: _categoryRepositoryMock.Object,
            validator: _validatorMock.Object,
            logger: _loggerMock.Object
        );
    }

    [Fact(DisplayName = "Given valid category creation request, should create category successfully")]
    public async Task GivenValidCategoryCreationRequest_ShouldCreateCategorySuccessfully()
    {
        var request = new CategoryCreationRequest { Name = "Action" };

        var validationResult = new ValidationResult();
        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        var result = await _handler.Handle(request, default);
        _categoryRepositoryMock.Verify(repository => repository.SaveAsync(It.IsAny<Category>()), Times.Once);

        Assert.NotNull(result);

        Assert.True(result.IsSuccess);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        Assert.Equal("category created successfully.", result.Message);
    }

    [Fact(DisplayName = "Given invalid category creation request, should throw validation exception")]
    public async Task GivenInvalidCategoryCreationRequest_ShouldReturnBadRequest()
    {
        var request = new CategoryCreationRequest { Name = string.Empty };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("Name", "Category name is required."));

        _validatorMock.Setup(validator => validator.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, default));

        _categoryRepositoryMock.Verify(repository => repository.SaveAsync(It.IsAny<Category>()), Times.Never);
        _validatorMock.Verify(validator => validator.ValidateAsync(request, CancellationToken.None), Times.Once);
    }
}
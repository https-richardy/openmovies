namespace OpenMovies.TestingSuite.ControllersTestSuite;

public sealed class CategoryControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CategoryController _controller;
    private readonly Fixture _fixture;

    public CategoryControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CategoryController(_mediatorMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given a valid request, should return a 201 Created response")]
    public async Task GivenValidRequest_ShouldReturnAnAccountRegistrationResponse()
    {
        var request = _fixture.Create<CategoryCreationRequest>();
        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "category created successfully"
        };
        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(new Response
            {
                StatusCode = StatusCodes.Status201Created,
                Message = "category created successfully"
            });

        var response = await _controller.CreateCategoryAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response;

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse, objectResult.Value);

        Assert.NotNull(objectResultValue);
        Assert.Equal(StatusCodes.Status201Created, objectResultValue.StatusCode);
        Assert.Equal("category created successfully", objectResultValue.Message);
    }

    [Fact(DisplayName = "Given a valid request, should return a 200 OK response")]
    public async Task GivenValidRequest_ShouldReturnOkResponse()
    {
        var categoryId = 1;
        var request = new CategoryUpdateRequest
        {
            CategoryId = categoryId,
            Name = "Updated Category Name"
        };

        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "category updated successfully"
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.UpdateCategoryAsync(request, categoryId);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var actualResponse = Assert.IsType<Response>(objectResult.Value);

        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.Equal(expectedResponse.StatusCode, actualResponse.StatusCode);
        Assert.Equal(expectedResponse.Message, actualResponse.Message);

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);
    }

    [Fact(DisplayName = "Given an invalid request, should throw ValidationException")]
    public async Task GivenInvalidRequest_ShouldThrowValidationException()
    {
        var categoryId = 1;
        var request = new CategoryUpdateRequest
        {
            CategoryId = categoryId,
            Name = ""
        };

        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure(nameof(CategoryUpdateRequest.Name), "Name is required")
        });

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ThrowsAsync(new ValidationException(validationResult.Errors));

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateCategoryAsync(request, categoryId));

        Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Name is required");

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);
    }

    [Fact(DisplayName = "Given category does not exist, should return 404 NotFound")]
    public async Task GivenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        var categoryId = 1;
        var request = new CategoryUpdateRequest
        {
            CategoryId = categoryId,
            Name = "Updated Category Name"
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(new Response
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "category not found"
            });

        var result = await _controller.UpdateCategoryAsync(request, categoryId);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var actualResponse = Assert.IsType<Response>(objectResult.Value);

        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.Equal("category not found", actualResponse.Message);

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);
    }
}
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
}
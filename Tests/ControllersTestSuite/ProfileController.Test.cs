namespace OpenMovies.TestingSuite.ControllersTestSuite;

public sealed class ProfileControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ProfileController _controller;
    private readonly Fixture _fixture;

    public ProfileControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProfileController(_mediatorMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given a valid request, should return created 201 response.")]
    public async Task GivenValidRequest_ShouldReturnCreatedResponse()
    {
        #pragma warning disable CS8600 // disabling because in this scenario the avatar (IFormFile) goes null
        var request = _fixture.Build<ProfileCreationRequest>()
            .With(request => request.Avatar, (IFormFile)null)
            .Create();

        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Profile created successfully."
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.CreateProfileAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response;

        _mediatorMock.Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse, objectResult.Value);

        Assert.NotNull(objectResultValue);
        Assert.Equal(StatusCodes.Status201Created, objectResultValue.StatusCode);
        Assert.Equal("Profile created successfully.", objectResultValue.Message);
    }

    [Fact(DisplayName = "Given a valid request with avatar, it should return a response of 201 Created")]
    public async Task GivenValidRequestWithAvatar_ShouldReturnCreatedResponse()
    {
        var request = _fixture.Build<ProfileCreationRequest>()
            .With(request => request.Avatar, new Mock<IFormFile>().Object)
            .Create();

        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "Profile created successfully."
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.CreateProfileAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response;

        _mediatorMock.Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse, objectResult.Value);

        Assert.NotNull(objectResultValue);
        Assert.Equal(StatusCodes.Status201Created, objectResultValue.StatusCode);
        Assert.Equal("Profile created successfully.", objectResultValue.Message);
    }

    [Fact(DisplayName = "Dado uma exceção de MaxProfileCountReachedException, deve retornar resposta de 403 Forbidden")]
    public async Task GivenMaxProfileCountReachedException_ShouldReturnForbiddenResponse()
    {
        var request = _fixture.Build<ProfileCreationRequest>()
            .With(request => request.Avatar, new Mock<IFormFile>().Object)
            .Create();

        var userId = Guid.NewGuid().ToString();
        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Message = "Maximum profile count reached."
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.CreateProfileAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response;

        _mediatorMock.Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse.StatusCode, objectResult.StatusCode);

        Assert.NotNull(objectResultValue);
        Assert.Equal(expectedResponse.StatusCode, objectResultValue.StatusCode);
    }
}
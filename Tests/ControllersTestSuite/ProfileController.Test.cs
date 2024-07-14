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

    [Fact(DisplayName = "Given a valid request, should return 200 OK with profiles")]
    public async Task GivenValidRequest_ShouldReturnOkWithProfiles()
    {
        var profiles = _fixture
            .CreateMany<ProfileInformation>(3)
            .ToList();

        var response = _fixture.Build<Response<IEnumerable<ProfileInformation>>>()
            .With(request => request.StatusCode, StatusCodes.Status200OK)
            .With(request => request.Data, profiles)
            .With(request => request.Message, "Profiles successfully recovered.")
            .Create();

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<ProfilesRetrievalRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

        var result = await _controller.GetUserProfilesAsync();

        var objectResult = Assert.IsType<ObjectResult>(result);
        var actualResponse = Assert.IsType<Response<IEnumerable<ProfileInformation>>>(objectResult.Value);

        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        Assert.NotNull(actualResponse);
        Assert.NotNull(actualResponse.Data);

        Assert.Equal(expected: objectResult.Value, actual: actualResponse);

        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<ProfilesRetrievalRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
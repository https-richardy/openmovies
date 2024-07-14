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

    [Fact(DisplayName = "Given a valid profile selection request, should return 200 OK with authentication response")]
    public async Task GivenValidProfileSelectionRequest_ShouldReturnOkWithAuthenticationResponse()
    {
        var responseData = new AuthenticationResponse { Token = "mocked.jwt.token" };

        var response = _fixture.Build<Response<AuthenticationResponse>>()
            .With(request => request.Data, responseData)
            .With(request => request.StatusCode, StatusCodes.Status200OK)
            .With(request => request.Message, "Profile successfully selected.")
            .Create();

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<ProfileSelectionRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        var result = await _controller.SelectProfileAsync(1);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var actualResponse = Assert.IsType<Response<AuthenticationResponse>>(objectResult.Value);

        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        Assert.Equal(response, objectResult.Value);
        Assert.Equal(expected: objectResult.Value, actual: actualResponse);

        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<ProfileSelectionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Given an invalid profile selection request, should return 404 Not Found")]
    public async Task GivenInvalidProfileSelectionRequest_ShouldReturnNotFound()
    {
        var response = _fixture.Build<Response<AuthenticationResponse>>()
            .With(request => request.StatusCode, StatusCodes.Status404NotFound)
            .With(request => request.Message, "Profile not found.")
            .Create();

        _mediatorMock.Setup(mediator => mediator.Send(It.IsAny<ProfileSelectionRequest>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        var result = await _controller.SelectProfileAsync(99);

        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.Equal(response, objectResult.Value);

        _mediatorMock.Verify(mediator => mediator.Send(It.IsAny<ProfileSelectionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "DeleteProfileAsync should return correct status code and response")]
    public async Task DeleteProfileAsync_ShouldReturnCorrectStatusCodeAndResponse()
    {
        var profileId = 1;
        var deletionRequest = new ProfileDeletionRequest { ProfileId = profileId };
        var expectedResult = new Response { StatusCode = StatusCodes.Status200OK, Message = "Profile deleted successfully." };

        _mediatorMock
            .Setup(mediator => mediator.Send(deletionRequest, default))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeleteProfileAsync(profileId);

        var statusCode = (result as ObjectResult)?.StatusCode;
        var response = (result as ObjectResult)?.Value as Response;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, statusCode);
        Assert.Equal(expectedResult.Message, response?.Message);
    }

    [Fact(DisplayName = "Given an invalid profile id to delete, should return 404 Not Found")]
    public async Task GivenInvalidProfileIdToDelete_ShouldReturnNotFound()
    {
        var profileId = 99;

        var deletionRequest = new ProfileDeletionRequest { ProfileId = profileId };
        var expectedResult = new Response { StatusCode = StatusCodes.Status404NotFound, Message = "Profile not found." };

        _mediatorMock
            .Setup(mediator => mediator.Send(deletionRequest, default))
            .ReturnsAsync(expectedResult);

        var result = await _controller.DeleteProfileAsync(profileId);

        var statusCode = (result as ObjectResult)?.StatusCode;
        var response = (result as ObjectResult)?.Value as Response;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, statusCode);
        Assert.Equal(expectedResult.Message, response?.Message);
    }
}
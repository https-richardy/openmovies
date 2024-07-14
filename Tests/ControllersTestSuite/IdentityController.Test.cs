namespace OpenMovies.TestingSuite.ControllersTestSuite;

public sealed class IdentityControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IdentityController _controller;
    private readonly IFixture _fixture;

    public IdentityControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new IdentityController(_mediatorMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given a valid request, should return a 201 Created response")]
    public async Task GivenValidRequest_ShouldReturnAnAccountRegistrationResponse()
    {
        var request = _fixture.Create<AccountRegistrationRequest>();
        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "account created successfully"
        };

        _mediatorMock.Setup(mediator => mediator.Send(
            request,
            CancellationToken.None
        ))
        .ReturnsAsync(expectedResponse);

        var response = await _controller.RegisterAccountAsync(request);
        var objectResult = response as ObjectResult;

        _mediatorMock.Verify(mediator => mediator.Send(
            request,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse, objectResult.Value);
    }

    [Fact(DisplayName = "Given an invalid request, should return a 400 Bad Request response")]
    public async Task GivenInvalidRequest_ShouldReturn400BadRequestResponse()
    {
        var request = new AccountRegistrationRequest();

        _mediatorMock.Setup(mediator => mediator.Send(
            request,
            default
        ))
        .ThrowsAsync(new ValidationException("Validation error message"));

        await Assert.ThrowsAsync<ValidationException>(() => _controller.RegisterAccountAsync(request));

        _mediatorMock.Verify(mediator => mediator.Send(
            request,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact(DisplayName = "Given valid credentials, should return a 200 OK response with token")]
    public async Task GivenValidCredentials_ShouldReturn200OKResponseWithToken()
    {
        var request = new AuthenticationCredentials();
        var expectedResponse = new Response<AuthenticationResponse>
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "Authentication successful.",
            Data = new AuthenticationResponse { Token = "mocked.jwt.token" }
        };

        _mediatorMock.Setup(mediator => mediator.Send(
            request,
            default
        ))
        .ReturnsAsync(expectedResponse);

        var response = await _controller.AuthenticateAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response<AuthenticationResponse>;

        _mediatorMock.Verify(mediator => mediator.Send(
            request,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.NotNull(objectResultValue);
        Assert.NotNull(objectResultValue.Data);

        Assert.Equal(expectedResponse.StatusCode, objectResult.StatusCode);
        Assert.Equal(expectedResponse, objectResult.Value);
        Assert.Equal("mocked.jwt.token", objectResultValue.Data.Token);
    }

    [Fact(DisplayName = "Given invalid credentials, should return a 401 Unauthorized response")]
    public async Task GivenInvalidCredentials_ShouldReturn401UnauthorizedResponse()
    {
        var request = _fixture.Create<AuthenticationCredentials>();
        var expectedResponse = new Response<AuthenticationResponse>
        {
            Data = null,
            StatusCode = StatusCodes.Status401Unauthorized,
            Message = "Invalid credentials."
        };

        _mediatorMock.Setup(mediator => mediator.Send(
            request,
            default
        ))
        .ReturnsAsync(expectedResponse);

        var response = await _controller.AuthenticateAsync(request);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response<AuthenticationResponse>;

        _mediatorMock.Verify(mediator => mediator.Send(
            request,
            It.IsAny<CancellationToken>()
        ), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.NotNull(objectResultValue);
        Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
        Assert.Equal("Invalid credentials.", objectResultValue.Message);
    }
}
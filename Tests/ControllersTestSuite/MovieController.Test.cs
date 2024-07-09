namespace OpenMovies.TestingSuite.ControllersTestSuite;

public sealed class MovieControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MovieController _controller;
    private readonly Fixture _fixture;

    public MovieControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MovieController(_mediatorMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Given a valid request, should return a 201 Created response for CreateMovieAsync")]
    public async Task GivenValidRequest_ShouldReturnCreatedResponseForCreateMovieAsync()
    {
        var request = new MovieCreationRequest
        {
            Title = "The Matrix",
            Synopsis = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
            VideoSource = "https://www.youtube.com/watch?v=m8e-FF8MsqU",
            ReleaseYear = 1999,
            Image = new Mock<IFormFile>().Object,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status201Created,
            Message = "movie created successfully"
        };
        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.CreateMovieAsync(request);
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
        Assert.Equal("movie created successfully", objectResultValue.Message);
    }

    [Fact(DisplayName = "Given an invalid request, should throw ValidationException for CreateMovieAsync")]
    public async Task GivenInvalidRequest_ShouldThrowValidationExceptionForCreateMovieAsync()
    {
        var request = new MovieCreationRequest
        {
            Title = "",
            Synopsis = "",
            VideoSource = "invalid-url",
            ReleaseYear = -1,
            Image = new Mock<IFormFile>().Object,
            DurationInMinutes = 0,
            CategoryId = 0
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ThrowsAsync(new ValidationException("Invalid request"));

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.CreateMovieAsync(request));

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.Equal("Invalid request", exception.Message);
    }

    [Fact(DisplayName = "Given a valid request, should return a 200 OK response for UpdateMovieAsync")]
    public async Task GivenValidRequest_ShouldReturnOkResponseForUpdateMovieAsync()
    {
        var request = new MovieUpdateRequest
        {
            Title = "The Matrix",
            Synopsis = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
            VideoSource = "https://www.youtube.com/watch?v=m8e-FF8MsqU",
            ReleaseYear = 1999,
            Image = new Mock<IFormFile>().Object,
            DurationInMinutes = 120,
            CategoryId = 1,
            MovieId = 1
        };

        var expectedResponse = new Response
        {
            StatusCode = StatusCodes.Status200OK,
            Message = "movie updated successfully"
        };
        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ReturnsAsync(expectedResponse);

        var response = await _controller.UpdateMovieAsync(request, request.MovieId);
        var objectResult = response as ObjectResult;
        var objectResultValue = objectResult?.Value as Response;

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        Assert.NotNull(objectResult);
        Assert.Equal(expectedResponse, objectResult.Value);

        Assert.NotNull(objectResultValue);
        Assert.Equal(StatusCodes.Status200OK, objectResultValue.StatusCode);
        Assert.Equal("movie updated successfully", objectResultValue.Message);
    }

    [Fact(DisplayName = "Given an invalid request, should throw ValidationException for UpdateMovieAsync")]
    public async Task GivenInvalidRequest_ShouldThrowValidationExceptionForUpdateMovieAsync()
    {
        var request = new MovieUpdateRequest
        {
            Title = "",
            Synopsis = "",
            VideoSource = "invalid-url",
            ReleaseYear = -1,
            Image = new Mock<IFormFile>().Object,
            DurationInMinutes = 0,
            CategoryId = 0,
            MovieId = 0
        };

        _mediatorMock
            .Setup(mediator => mediator.Send(request, default))
            .ThrowsAsync(new ValidationException("Invalid request"));

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.UpdateMovieAsync(request, request.MovieId));

        _mediatorMock
            .Verify(mediator => mediator.Send(request, default), Times.Once);

        Assert.Equal("Invalid request", exception.Message);
    }
}
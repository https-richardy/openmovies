namespace OpenMovies.TestingSuite.MappingTestSuite;

public sealed class MovieMappingTest
{
    private readonly IServiceCollection _services;
    private readonly IFixture _fixture;

    public MovieMappingTest()
    {
        _services = new ServiceCollection();
        _services.AddMapping();

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Should map a MovieCreationRequest to a Movie entity correctly.")]
    public void ShouldMapAMovieCreationRequestToAMovieEntityCorrectly()
    {
        var movieCreationRequest = new MovieCreationRequest
        {
            Title = "Title",
            Synopsis = "Synopsis",
            VideoSource = "https://www.youtube.com/watch?v=1234",
            ReleaseYear = 2022,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var mappedMovie = TinyMapper.Map<Movie>(movieCreationRequest);

        Assert.Equal(movieCreationRequest.Title, mappedMovie.Title);
        Assert.Equal(movieCreationRequest.Synopsis, mappedMovie.Synopsis);
        Assert.Equal(movieCreationRequest.VideoSource, mappedMovie.VideoSource);
        Assert.Equal(movieCreationRequest.ReleaseYear, mappedMovie.ReleaseYear);
        Assert.Equal(movieCreationRequest.DurationInMinutes, mappedMovie.DurationInMinutes);
    }

    [Fact(DisplayName = "Should map a MovieUpdateRequest to a Movie entity correctly.")]
    public void ShouldMapAMovieUpdateRequestToAMovieEntityCorrectly()
    {
        var movieUpdateRequest = new MovieUpdateRequest
        {
            Title = "Title",
            Synopsis = "Synopsis",
            VideoSource = "https://www.youtube.com/watch?v=1234",
            ReleaseYear = 2022,
            DurationInMinutes = 120,
            CategoryId = 1,
            MovieId = 1
        };

        var mappedMovie = TinyMapper.Map<Movie>(movieUpdateRequest);

        Assert.Equal(movieUpdateRequest.Title, mappedMovie.Title);
        Assert.Equal(movieUpdateRequest.Synopsis, mappedMovie.Synopsis);
        Assert.Equal(movieUpdateRequest.VideoSource, mappedMovie.VideoSource);
        Assert.Equal(movieUpdateRequest.ReleaseYear, mappedMovie.ReleaseYear);
        Assert.Equal(movieUpdateRequest.DurationInMinutes, mappedMovie.DurationInMinutes);
    }
}
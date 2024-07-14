namespace OpenMovies.TestingSuite.ValidatorsTestSuite.MovieValidators;

public sealed class UpdateMovieValidatorTest
{
    private readonly IValidator<MovieUpdateRequest> _validator;

    public UpdateMovieValidatorTest()
    {
        _validator = new MovieUpdateValidator();
    }

    [Fact(DisplayName = "Given a invalid movie id, should have validation error")]
    public async Task GivenInvalidMovieId_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1,
            MovieId = 0
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie id must be greater than 0.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty title, should have validation error")]
    public async Task GivenEmptyTitle_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = string.Empty,
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie title is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a title with more than 120 characters, should have validation error")]
    public async Task GivenTitleExceedingMaxLength_ShouldHaveValidationError()
    {
        var longTitle = new string('A', 121);
        var payload = new MovieUpdateRequest
        {
            Title = longTitle,
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie title must be at most 120 characters.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty synopsis, should have validation error")]
    public async Task GivenEmptySynopsis_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = string.Empty,
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie synopsis is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a synopsis with more than 1000 characters, should have validation error")]
    public async Task GivenSynopsisExceedingMaxLength_ShouldHaveValidationError()
    {
        var longSynopsis = new string('A', 1001);
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = longSynopsis,
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie synopsis must be at most 1000 characters.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty video source, should have validation error")]
    public async Task GivenEmptyVideoSource_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = string.Empty,
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie video source (CDN link) is required.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty release year, should have validation error")]
    public async Task GivenEmptyReleaseYear_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = 0,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie release year is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a release year in the future, should have validation error")]
    public async Task GivenFutureReleaseYear_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year + 1,
            DurationInMinutes = 120,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie release year must be in the past.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty duration in minutes, should have validation error")]
    public async Task GivenEmptyDurationInMinutes_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 0,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie duration in minutes is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a duration in minutes less than or equal to 0, should have validation error")]
    public async Task GivenDurationInMinutesLessThanOrEqualToZero_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = -1,
            CategoryId = 1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie duration in minutes must be greater than 0.", errorMessages);
    }

    [Fact(DisplayName = "Given an empty category ID, should have validation error")]
    public async Task GivenEmptyCategoryId_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 0
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie category is required.", errorMessages);
    }

    [Fact(DisplayName = "Given a category ID less than or equal to 0, should have validation error")]
    public async Task GivenCategoryIdLessThanOrEqualToZero_ShouldHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = -1
        };

        var result = await _validator.ValidateAsync(payload);
        var errorMessages = result.Errors.Select(error => error.ErrorMessage);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Contains("Movie category must be greater than 0.", errorMessages);
    }

    [Fact(DisplayName = "Given a valid payload, should not have validation error")]
    public async Task GivenValidPayload_ShouldNotHaveValidationError()
    {
        var payload = new MovieUpdateRequest
        {
            Title = "Movie title",
            Synopsis = "Movie synopsis",
            VideoSource = "https://example.com/video.mp4",
            ReleaseYear = DateTime.Now.Year,
            DurationInMinutes = 120,
            CategoryId = 1,
            MovieId = 1
        };

        var result = await _validator.ValidateAsync(payload);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
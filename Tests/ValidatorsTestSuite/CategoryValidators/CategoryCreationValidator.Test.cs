namespace OpenMovies.TestingSuite.ValidatorsTestSuite.CategoryValidators;

public sealed class CategoryCreationValidatorTest
{
    private readonly IValidator<CategoryCreationRequest> _validator;

    public CategoryCreationValidatorTest()
    {
        _validator = new CategoryCreationValidator();
    }

    [Fact(DisplayName = "Given valid category creation request, should not have validation error")]
    public async Task GivenValidCategoryCreationRequest_ShouldNotHaveValidationError()
    {
        var request = new CategoryCreationRequest
        {
            Name = "Action"
        };

        var validationResult = await _validator.ValidateAsync(request);
        Assert.True(validationResult.IsValid);
    }

    [Fact(DisplayName = "Given empty category name, should have validation error")]
    public async Task GivenEmptyCategoryName_ShouldHaveValidationError()
    {
        var request = new CategoryCreationRequest
        {
            Name = string.Empty
        };

        var validationResult = await _validator.ValidateAsync(request);
        Assert.False(validationResult.IsValid);
        Assert.Contains("Category name is required.", validationResult.Errors.Select(error => error.ErrorMessage));
    }

    [Fact(DisplayName = "Given category name with less than 3 characters, should have validation error")]
    public async Task GivenCategoryNameWithLessThan3Characters_ShouldHaveValidationError()
    {
        var request = new CategoryCreationRequest
        {
            Name = "ab"
        };

        var validationResult = await _validator.ValidateAsync(request);
        Assert.False(validationResult.IsValid);
        Assert.Contains("Category name must be at least 3 characters.", validationResult.Errors.Select(error => error.ErrorMessage));
    }

    [Fact(DisplayName = "Given category name with more than 50 characters, should have validation error")]
    public async Task GivenCategoryNameWithMoreThan50Characters_ShouldHaveValidationError()
    {
        var request = new CategoryCreationRequest
        {
            Name = new string('A', 51)
        };

        var validationResult = await _validator.ValidateAsync(request);
        Assert.False(validationResult.IsValid);
        Assert.Contains("Category name must be at most 50 characters.", validationResult.Errors.Select(error => error.ErrorMessage));
    }
}
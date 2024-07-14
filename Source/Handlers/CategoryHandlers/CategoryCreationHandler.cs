namespace OpenMovies.WebApi.Handlers;

public sealed class CategoryCreationHandler(
    ICategoryRepository categoryRepository,
    IValidator<CategoryCreationRequest> validator,
    ILogger<CategoryCreationHandler> logger
) : IRequestHandler<CategoryCreationRequest, Response>
{
    public async Task<Response> Handle(
        CategoryCreationRequest request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = new Category { Name = request.Name };
        await categoryRepository.SaveAsync(category);

        logger.LogInformation("Category {CategoryName} created successfully.", category.Name);

        return new Response(
            statusCode: StatusCodes.Status201Created,
            message: "category created successfully."
        );
    }
}

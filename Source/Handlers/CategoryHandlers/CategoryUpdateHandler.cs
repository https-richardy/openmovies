namespace OpenMovies.WebApi.Handlers;

public sealed class CategoryUpdateHandler(
    ICategoryRepository categoryRepository,
    IValidator<CategoryUpdateRequest> validator,
    ILogger<CategoryUpdateHandler> logger
) : IRequestHandler<CategoryUpdateRequest, Response>
{
    public async Task<Response> Handle(
        CategoryUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "category not found"
            );

        category.Name = request.Name;
        await categoryRepository.SaveAsync(category);

        logger.LogInformation("Category {CategoryName} updated successfully.", category.Name);

        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "category updated successfully"
        );
    }
}
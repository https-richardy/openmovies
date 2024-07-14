namespace OpenMovies.WebApi.Payloads;

public sealed class CategoryDeletionHandler(
    ICategoryRepository categoryRepository,
    ILogger<CategoryDeletionHandler> logger
) : IRequestHandler<CategoryDeletionRequest, Response>
{
    public async Task<Response> Handle(
        CategoryDeletionRequest request,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return new Response(
                statusCode: StatusCodes.Status404NotFound,
                message: "category not found"
            );

        await categoryRepository.DeleteAsync(category);
        logger.LogInformation("Category with id `{CategoryId}` deleted successfully.", request.CategoryId);

        return new Response(
            statusCode: StatusCodes.Status200OK,
            message: "category deleted successfully"
        );
    }
}
namespace OpenMovies.WebApi.Payloads;

public sealed class CategoryRetrievalHandler(
    ICategoryRepository categoryRepository,
    ILogger<CategoryRetrievalHandler> logger
) : IRequestHandler<CategoryRetrievalRequest, Response<Category>>
{
    public async Task<Response<Category>> Handle(
        CategoryRetrievalRequest request,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null)
            return new Response<Category>(
                data: null,
                statusCode: StatusCodes.Status404NotFound,
                message: "category not found"
            );

        logger.LogInformation("Category {CategoryName} retrieved successfully.", category.Name);

        return new Response<Category>(
            data: category,
            statusCode: StatusCodes.Status200OK,
            message: "category retrieved successfully"
        );
    }
}
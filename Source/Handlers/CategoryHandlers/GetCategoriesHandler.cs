namespace OpenMovies.WebApi.Handlers;

public sealed class GetCategoriesHandler(
    ICategoryRepository categoryRepository,
    ILogger<GetCategoriesHandler> logger
) : IRequestHandler<GetCategoriesRequest, Response<IEnumerable<Category>>>
{
    public async Task<Response<IEnumerable<Category>>> Handle(
        GetCategoriesRequest request, 
        CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync();
        logger.LogInformation("Categories retrieved successfully.");

        return new Response<IEnumerable<Category>>(
            data: categories,
            statusCode: StatusCodes.Status200OK,
            message: "categories retrieved successfully"
        );
    }
}

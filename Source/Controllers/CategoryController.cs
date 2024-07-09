namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoryController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CreateCategoryAsync(CategoryCreationRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }
}
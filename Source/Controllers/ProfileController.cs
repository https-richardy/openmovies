namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Common")]
    public async Task<IActionResult> GetUserProfilesAsync()
    {
        var request = new ProfilesRetrievalRequest();

        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    [Authorize(Roles = "Common")]
    public async Task<IActionResult> CreateProfileAsync(ProfileCreationRequest request)
    {
        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{profileId}")]
    [Authorize(Roles = "Common")]
    public async Task<IActionResult> EditProfileAsync(ProfileEditingRequest request, [FromRoute] int profileId)
    {
        request.ProfileId = profileId;

        var response = await mediator.Send(request);
        return StatusCode(response.StatusCode, response);
    }
}
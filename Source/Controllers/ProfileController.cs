namespace OpenMovies.WebApi.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpPost("select/{profileId}")]
    [Authorize(Roles = "Common")]
    public async Task<IActionResult> SelectProfileAsync([FromRoute] int profileId)
    {
        var request = new ProfileSelectionRequest { ProfileId = profileId };
        var response = await mediator.Send(request);

        return StatusCode(response.StatusCode, response);
    }

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

    [HttpDelete("{profileId}")]
    [Authorize(Roles = "Common")]
    public async Task<IActionResult> DeleteProfileAsync(int profileId)
    {
        var response = await mediator.Send(new ProfileDeletionRequest
        {
            ProfileId = profileId
        });

        return StatusCode(response.StatusCode, response);
    }
}
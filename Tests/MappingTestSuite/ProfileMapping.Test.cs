namespace OpenMovies.TestingSuite.MappingTestSuite;

public sealed class ProfileMappingTest
{
    private readonly IServiceCollection _services;

    public ProfileMappingTest()
    {
        _services = new ServiceCollection();
        _services.AddMapping();
    }

    [Fact(DisplayName = "Should map a ProfileCreationRequest to a Profile entity correctly.")]
    public void ShouldMapAProfileCreationRequestToAProfileEntityCorrectly()
    {
        var profileCreationRequest = new ProfileCreationRequest
        {
            Name = "John Doe",
            IsChild = true
        };

        var mappedProfile = TinyMapper.Map<Profile>(profileCreationRequest);

        Assert.Equal(profileCreationRequest.Name, mappedProfile.Name);
        Assert.Equal(profileCreationRequest.IsChild, mappedProfile.IsChild);
        Assert.Null(mappedProfile.Avatar); // Since Avatar is ignored in mapping
    }

    [Fact(DisplayName = "Should map a ProfileEditingRequest to a Profile entity correctly.")]
    public void ShouldMapAProfileUpdateRequestToAProfileEntityCorrectly()
    {
        var profileUpdateRequest = new ProfileEditingRequest
        {
            Name = "Updated User",
            IsChild = false
        };

        var mappedProfile = TinyMapper.Map<Profile>(profileUpdateRequest);

        Assert.Equal(profileUpdateRequest.Name, mappedProfile.Name);
        Assert.Equal(profileUpdateRequest.IsChild, mappedProfile.IsChild);
        Assert.Null(mappedProfile.Avatar); // Assuming Avatar is ignored in mapping for update as well
    }
}
namespace OpenMovies.TestingSuite.ServicesTestSuite;

public sealed class UserContextServiceTest
{
    private readonly Mock<IHttpContextAccessor> _contextAccessorMock;
    private readonly Mock<ILogger<UserContextService>> _loggerMock;
    private readonly IUserContextService _userContextService;

    public UserContextServiceTest()
    {
        _contextAccessorMock = new Mock<IHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<UserContextService>>();

        _userContextService = new UserContextService(
            _contextAccessorMock.Object,
            _loggerMock.Object
        );
    }

    [Fact(DisplayName = "UserContextService: Should get user id from http context")]
    public void GetCurrentUserId_UserInContext_ReturnsUserId()
    {
        var userId = Guid.NewGuid().ToString();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");

        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };

        _contextAccessorMock.Setup(accessor => accessor.HttpContext)
            .Returns(context);

        var result = _userContextService.GetCurrentUserId();
        Assert.Equal(userId, result);
    }

    [Fact(DisplayName = "Should return null when there is no user in the context")]
    public void GetCurrentUserId_NoUserInContext_ReturnsNull()
    {
        #pragma warning disable CS8600 // this needs to be null and void in this scenario.
        _contextAccessorMock.Setup(accessor => accessor.HttpContext)
            .Returns((HttpContext)null);

        var result = _userContextService.GetCurrentUserId();
        Assert.Null(result);
    }

    [Fact(DisplayName = "It should return the user's principal when a user is in the context")]
    public void GetCurrentUserClaimsPrincipal_UserInContext_ReturnsClaimsPrincipal()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "John Doe"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };

        _contextAccessorMock.Setup(accessor => accessor.HttpContext)
            .Returns(context);

        var result = _userContextService.GetCurrentUserClaimsPrincipal();

        Assert.Equal(claimsPrincipal, result);

        Assert.Equal(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), result?.FindFirstValue(ClaimTypes.NameIdentifier));
        Assert.Equal(claimsPrincipal.FindFirstValue(ClaimTypes.Name), result?.FindFirstValue(ClaimTypes.Name));
        Assert.Equal(claimsPrincipal.FindFirstValue(ClaimTypes.Role), result?.FindFirstValue(ClaimTypes.Role));
    }

    [Fact(DisplayName = "It should return null when there is no user in the context")]
    public void GetCurrentUserClaimsPrincipal_NoUserInContext_LogsWarning()
    {
        _contextAccessorMock.Setup(accessor => accessor.HttpContext)
            .Returns((HttpContext)null);

        var result = _userContextService.GetCurrentUserClaimsPrincipal();
        Assert.Null(result);
    }
}
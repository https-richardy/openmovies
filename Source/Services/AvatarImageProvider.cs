namespace OpenMovies.WebApi.Services;

public sealed class AvatarImageProvider(IWebHostEnvironment webHostEnvironment) : IAvatarImageProvider
{
    private const string _avatarFolderName = "avatars";

    public string GetRandomDefaultAvatar()
    {
        var avatarFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "assets", _avatarFolderName);
        var avatarFilePaths = Directory.GetFiles(avatarFolderPath);

        var random = new Random();

        var randomIndex = random.Next(avatarFilePaths.Length);
        var selectedFilePath = avatarFilePaths[randomIndex];

        var fileName = Path.GetFileName(selectedFilePath);
        var relativePath = Path.Combine("assets", _avatarFolderName, fileName);

        return relativePath;
    }
}
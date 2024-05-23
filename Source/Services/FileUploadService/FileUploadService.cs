namespace OpenMovies.WebApi.Services;

public class FileUploadService : IFileUploadService
{
    private readonly FileUploadOptions _options;
    private const string _uploadsFolder = "uploads";

    /// <summary>
    /// Initializes a new instance of the <see cref="FileUploadService"/> class with default options.
    /// </summary>
    /// <param name="webHostEnvironment">The hosting environment.</param>
    /// <remarks>
    /// When using this overload, the service will be initialized with default options.
    /// The uploads directory will be set to the default specified in the web root path, typically wwwroot
    /// </remarks>
    public FileUploadService(IWebHostEnvironment webHostEnvironment)
    {
        // https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0

        /*
            Configuration of the file upload service.
            The uploads directory is defined as a subfolder within the wwwroot directory.
            This is done to ensure that files uploaded by users are stored
            in a location accessible by the application and that can be served through HTTP requests.
            Additionally, it's important to note that ASP.NET Core's static file middleware
            by default serves files from the wwwroot directory, therefore, place uploads within this
            directory facilitates the management and availability of files for the application.
        */
        _options = new FileUploadOptions
        {
            UploadsDirectory = Path.Combine(webHostEnvironment.WebRootPath, _uploadsFolder),
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileUploadService"/> class with custom options.
    /// </summary>
    /// <param name="options">The file upload options.</param>
    public FileUploadService(FileUploadOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    ///  Uploads a file asynchronously.
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidFileExtensionException"></exception>
    /// <exception cref="FileSizeLimitExceededException"></exception>
    /// <exception cref="FileOverwriteNotAllowedException"></exception>
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_options.AllowedExtensions.Contains(fileExtension))
            throw new InvalidFileExtensionException("File extension is not allowed.");

        if (file.Length > _options.MaxFileSize)
            throw new FileSizeLimitExceededException("File size exceeds the maximum allowed size.");

        /* Ensure uploads directory exists */
        Directory.CreateDirectory(_options.UploadsDirectory);

        var fileName = _options.GenerateUniqueFileNames
            ? Guid.NewGuid().ToString()
            : Path.GetFileNameWithoutExtension(file.FileName);

        fileName += Path.GetExtension(file.FileName);

        var filePath = Path.Combine(_options.UploadsDirectory, fileName);

        /* Check if the file should be overwritten */
        if (_options.OverwriteExistingFiles == false && File.Exists(filePath))
            throw new FileOverwriteNotAllowedException("File already exists and overwriting is not allowed.");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(_uploadsFolder, fileName);
    }
}
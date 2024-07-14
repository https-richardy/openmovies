namespace OpenMovies.WebApi.Services;

public static class FileUploadServiceExtension
{
    /// <summary>
    /// Adds the <see cref="FileUploadService"/> to the specified <see cref="IServiceCollection"/> with default options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <remarks>
    /// This method adds the <see cref="FileUploadService"/> to the service collection using the default options.
    /// The default options include configuring the upload directory as a subfolder within the web root path, typically defined in the wwwroot directory.
    /// By default, the service will use the <see cref="IWebHostEnvironment"/> to obtain the web root path.
    /// </remarks>
    public static void AddFileUploadService(this IServiceCollection services)
    {
        services.AddTransient<IFileUploadService, FileUploadService>();
    }

    /// <summary>
    /// Adds the <see cref="FileUploadService"/> to the specified <see cref="IServiceCollection"/> with custom options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="configureOptions">A delegate to configure the <see cref="FileUploadOptions"/>.</param>
    /// <remarks>
    /// This method allows the addition of the <see cref="FileUploadService"/> to the service collection
    /// with custom options specified through the <paramref name="configureOptions"/> delegate.
    /// The delegate provides a way to configure various aspects of file upload functionality,
    /// such as specifying the upload directory, allowed file extensions, maximum file size, etc.
    /// </remarks>
    public static void AddFileUploadService(this IServiceCollection services, Action<FileUploadOptions> configureOptions)
    {
        if (configureOptions is null)
            throw new ArgumentNullException(nameof(configureOptions));

        var options = new FileUploadOptions();
        configureOptions(options);

        services.AddTransient<IFileUploadService>(_ => new FileUploadService(options));
    }
}
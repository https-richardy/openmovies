namespace OpenMovies.WebApi.Services;

/// <summary>
/// Represents options for configuring file uploads.
/// </summary>
/// <remarks>
/// Provides settings for specifying upload directory, allowed file extensions, 
/// handling existing files, generating unique file names, and setting maximum file size.
/// By default, the upload directory is empty, allowed extensions include common image 
/// and media formats, existing files are not overwritten, unique file names are generated,
/// and the maximum file size is set to 10 MB.
/// </remarks>
public record FileUploadOptions
{
    /// <summary>
    /// Gets or sets the directory where uploads will be stored.
    /// </summary>
    public string UploadsDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the allowed file extensions for uploads.
    /// </summary>
    /// <remarks>
    /// By default, the allowed file extensions include common image and media formats such as 
    /// .jpg, .jpeg, .png, .gif, .mp4, and .mp3. However, This array can be customized as needed.
    /// </remarks>
    public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mp3" };

    /// <summary>
    /// Gets or sets a value indicating whether existing files should be overwritten.
    /// </summary>
    /// <remarks>
    /// By default, existing files are not overwritten. This behavior can be customized
    /// by setting this property to true if existing files should be overwritten.
    /// </remarks>
    public bool OverwriteExistingFiles { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether unique file names should be generated.
    /// </summary>
    /// <remarks>
    /// By default, unique file names are generated for uploaded files. You can customize
    /// this behavior by setting this property to false if unique file names are not desired.
    /// </remarks>
    public bool GenerateUniqueFileNames { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum allowed file size (in bytes).
    /// </summary>
    /// <remarks>
    /// By default, the maximum allowed file size is set to 10 MB. 
    /// You can customize this value according to their specific requirements, 
    /// allowing for different maximum file sizes to be set for uploads.
    /// </remarks>
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10 MB
}
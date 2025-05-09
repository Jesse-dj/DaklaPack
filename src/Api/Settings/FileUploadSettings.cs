namespace Api.Settings;

public class FileUploadSettings
{
    public string[] AllowedExtensions { get; set; } = [];
    public long FileSizeLimitInBytes { get; set; }
}

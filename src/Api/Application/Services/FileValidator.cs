namespace Api.Application.Services;

public static class FileValidator
{
    public static bool FileExceedsFileSizeLimit(Stream file, long fileSizeLimit)
    {
        return file.Length > fileSizeLimit;
    }

    public static bool IsValidFileExtension(string fileExtension, string[] allowedExtensions)
    {
        return !string.IsNullOrEmpty(fileExtension) && allowedExtensions.Contains(fileExtension);
    }
}

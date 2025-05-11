using Api.Settings;
using Ardalis.Result;
using Microsoft.Extensions.Options;

namespace Api.Application.Services;

public sealed class FileService(
    IFileMutationService mutationService,
    IOptions<FileUploadSettings> options) : IFileService
{
    private readonly IFileMutationService _mutationService = mutationService;
    private readonly FileUploadSettings _uploadSettings = options.Value;

    public async Task<Result<MutatedFileResult?>> UploadFileAsync(Stream file, string fileName, CancellationToken cancellationToken = default)
    {
        if (FileValidator.FileExceedsFileSizeLimit(file, _uploadSettings.FileSizeLimitInBytes))
        {
            return Result.Error("File size exceeds the limit.");
        }

        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

        if (!FileValidator.IsValidFileExtension(fileExtension, _uploadSettings.AllowedExtensions))
        {
            return Result.Error("Invalid file type. Only .txt files are allowed.");
        }

        var mutatedFile = await _mutationService.MutateFileAsync(file, cancellationToken);

        return Result.Success<MutatedFileResult?>(new MutatedFileResult(mutatedFile, $"{Path.GetRandomFileName()}.txt"));
    }
}

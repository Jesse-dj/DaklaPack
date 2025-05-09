using Ardalis.Result;

namespace Api.Application.Services;

internal interface IFileService
{
    Task<Result<MutatedFileResult?>> UploadFileAsync(Stream file, string fileName, CancellationToken cancellationToken = default);
}
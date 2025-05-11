using Api.Application.Services;
using Ardalis.Result;

namespace Api.Application.Abstractions;

internal interface IFileService
{
    Task<Result<MutatedFileResult?>> UploadFileAsync(Stream file, string fileName, CancellationToken cancellationToken = default);
}
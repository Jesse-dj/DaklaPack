namespace Api.Application.Services;

public interface IFileMutationService
{
    Task<Stream> MutateFileAsync(Stream file, CancellationToken cancellationToken = default);
}
namespace Api.Application.Abstractions;

public interface IFileMutationService
{
    Task<Stream> MutateFileAsync(Stream file, CancellationToken cancellationToken = default);
}
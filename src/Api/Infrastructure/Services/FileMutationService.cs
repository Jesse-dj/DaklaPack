using System.Text;
using Api.Application.Abstractions;

namespace Api.Infrastructure.Services;

public sealed class FileMutationService(TimeProvider timeProvider) : IFileMutationService
{
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<Stream> MutateFileAsync(Stream file, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(file);
        var originalContent = await reader.ReadToEndAsync(cancellationToken);

        var mutatedContent = $"{originalContent}\nDate: {_timeProvider.GetUtcNow()}\nToken: {Guid.NewGuid()}";
        return new MemoryStream(Encoding.UTF8.GetBytes(mutatedContent));
    }
}

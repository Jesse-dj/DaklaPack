using Api.Application.Services;
using Api.Settings;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Api.UnitTests.Application.Services;

public class FileServiceTests
{
    private readonly IFileMutationService _mutationServiceMock;
    private readonly FileUploadSettings _fileUploadSettings;
    private readonly FileService _fileService;
    public FileServiceTests()
    {
        _mutationServiceMock = Substitute.For<IFileMutationService>();
        _fileUploadSettings = new FileUploadSettings
        {
            FileSizeLimitInBytes = 1024 * 1024, // 1 MB
            AllowedExtensions = new[] { ".txt" }
        };
        var options = Options.Create(_fileUploadSettings);
        _fileService = new FileService(_mutationServiceMock, options);
    }

    [Fact]
    public async Task UploadFileAsync_FileExceedsSizeLimit_ReturnsError()
    {
        // Arrange
        using var fileStream = new MemoryStream(new byte[_fileUploadSettings.FileSizeLimitInBytes + 1]);
        var fileName = "test.txt";
        // Act
        var result = await _fileService.UploadFileAsync(fileStream, fileName);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle("File size exceeds the limit.");
    }

    [Fact]
    public async Task UploadFileAsync_InvalidFileExtension_ReturnsError()
    {
        // Arrange
        using var fileStream = new MemoryStream(new byte[100]);
        var fileName = "test.jpg";
        // Act
        var result = await _fileService.UploadFileAsync(fileStream, fileName);
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle("Invalid file type. Only .txt files are allowed.");
    }
}

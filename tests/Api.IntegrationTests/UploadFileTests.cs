using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using Microsoft.Extensions.Time.Testing;

namespace Api.IntegrationTests;

public class UploadFileTests(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task UploadFile_ShouldReturnBadRequest_WhenFileIsNotTxt()
    {
        // Arrange
        var client = _factory.CreateClient();
        var fileContent = "This is a test file.";

        MultipartFormDataContent content = CreateFormDataContent(Encoding.UTF8.GetBytes(fileContent), "file.txt");
        // Act
        var response = await client.PostAsync("/file/upload", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UploadFile_ShouldReturnMutatedFile_WhenTxtFileUploaded()
    {
        var dateTimeOff = new DateTimeOffset(2023, 10, 1, 17, 43, 12, TimeSpan.Zero);

        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var timeProvider = (FakeTimeProvider)scopedServices.GetRequiredService<TimeProvider>();

            timeProvider.SetUtcNow(dateTimeOff);
        }

        // Arrange
        var client = _factory.CreateClient();

        var fileContent = "This is a test file.";

        MultipartFormDataContent content = CreateFormDataContent(Encoding.UTF8.GetBytes(fileContent), "file.txt");

        // Act
        var response = await client.PostAsync("/file/upload", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/plain",
            response.Content.Headers.ContentType.ToString());

        //check if the datetime is in the file content
        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        var fileString = Encoding.UTF8.GetString(fileBytes);
        Assert.Contains(dateTimeOff.ToString(), fileString);
    }

    private static MultipartFormDataContent CreateFormDataContent(byte[] fileBytes, string fileName)
    {
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Text.Plain);

        return new MultipartFormDataContent()
        {
            { fileContent, "file", fileName }
        };
    }
}
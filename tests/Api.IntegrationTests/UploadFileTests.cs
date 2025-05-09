using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.IntegrationTests;

public class UploadFileTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task UploadFile_ShouldReturnMutatedFile_WhenTxtFileUploaded()
    {
        // Arrange
        var client = _factory.CreateClient();

        var fileContent = "This is a test file.";

        MultipartFormDataContent content = CreateFormDataContent(Encoding.UTF8.GetBytes(fileContent));

        // Act
        var response = await client.PostAsync("/file/upload", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/plain; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }

    private static MultipartFormDataContent CreateFormDataContent(byte[] fileBytes)
    {
        var fileContent = new ByteArrayContent(fileBytes);
        var content = new MultipartFormDataContent()
        {
            { fileContent , "file" }
        };
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");

        return content;
    }
}
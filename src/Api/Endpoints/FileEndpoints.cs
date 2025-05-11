using System.Net.Mime;
using Api.Application.Services;
using Api.Settings;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Endpoints;

public static class FileEndpoints
{
    public static void MapFileEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/file/upload", async ([FromForm] IFormFile file, IFileService fileService, IOptions<FileUploadSettings> options, CancellationToken cancellationToken) =>
        {
            var result = await fileService.UploadFileAsync(file.OpenReadStream(), file.FileName, cancellationToken);

            if (result.IsError())
            {
                var problem = new ProblemDetails()
                {
                    Title = result.Status.ToString(),
                    Detail = string.Join("; ", result.Errors),
                    Status = StatusCodes.Status400BadRequest
                };
                return Results.Problem(problem);
            }

            return Results.File(result.Value!.File, MediaTypeNames.Text.Plain, result.Value.FileName);
        })
        .Produces(StatusCodes.Status200OK, contentType: MediaTypeNames.Text.Plain, responseType: typeof(Stream))
        .Produces(StatusCodes.Status400BadRequest, responseType: typeof(ProblemDetails))
        .WithName("MutateFile")
        .WithOpenApi(op =>
        {
            op.Summary = "Upload a text file and get a mutated version.";

            return op;
        })
        .DisableAntiforgery();
    }
}

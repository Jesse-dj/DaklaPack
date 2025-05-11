using Api.Application.Abstractions;
using Api.Application.Services;
using Api.Endpoints;
using Api.Infrastructure.Services;
using Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.Configure<FileUploadSettings>(
    builder.Configuration.GetSection("FileUpload"));

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileMutationService, FileMutationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.MapFileEndpoints();

await app.RunAsync();

public partial class Program { }
using DotNetAI.API.Service;
using DotNetAI.Extensions;
using DotNetAI.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddOpenAI();

builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<RecipeService>();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddSingleton<TranscriptionService>();

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new()
        {
            Title = ".NET AI API",
            Version = "v1",
            Description = "This API provides AI-based features such as chat, image generation, recipe creation and audio transcription.",
            Contact = new()
            {
                Name = "Marcelo Moura",
                Email = "",
                Url = new Uri("https://br.linkedin.com/in/marcelo-henrique-9b084b165")
            },
            License = new()
            {
                Name = "Apache 2 License",
                Url = new Uri("https://github.com/marcelo-moura")
            },
            TermsOfService = new Uri("https://github.com/marcelo-moura")
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = ".NET AI API";
        options.Theme = ScalarTheme.Default;
        options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();

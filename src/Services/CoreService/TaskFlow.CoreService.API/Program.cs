using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using TaskFlow.CoreService.Application.Shared;
using TaskFlow.CoreService.Infrastructure.Postgres.Shared;
using TaskFlow.CoreService.Presentation.Modules.TodoItems;
using TaskFlow.CoreService.Presentation.Shared;
using TaskFlow.TaskService.Domain.TodoItems;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TaskFlow CoreService API",
    });
    
    var basePath = AppContext.BaseDirectory;
    
    string[] xmlFiles = [
        $"{typeof(UpdateTodoTitleRequest).Assembly.GetName().Name}.xml",
        $"{typeof(Todo).Assembly.GetName().Name}.xml"
    ];

    foreach (var fileName in xmlFiles)
    {
        var path = Path.Combine(basePath, fileName);
        if (File.Exists(path))
        {
            options.IncludeXmlComments(path);
        }
    }
    
    options.DescribeAllParametersInCamelCase(); 
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddPresentation(configuration);
builder.Services.AddInfrastructurePostgres(configuration);
builder.Services.AddApplication(configuration);

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.BuildPresentation();

await app.RunAsync();

using Asp.Versioning;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    })
    .AddOpenApi(options =>
    {
        options.Document.AddSchemaTransformer((schema, context, ct) =>
        {
            var type = Nullable.GetUnderlyingType(context.JsonTypeInfo.Type)
                ?? context.JsonTypeInfo.Type;

            if (!type.IsEnum) return Task.CompletedTask;

            schema.Type = JsonSchemaType.String;

            schema.Enum = [.. Enum.GetNames(type).Select(
                name => JsonValue.Create(name))
            ];
            return Task.CompletedTask;
        });
        options.Document.AddOperationTransformer((operation, context, ct) =>
        {
            if (operation.Parameters is null) return Task.CompletedTask;

            foreach (var parameter in operation.Parameters)
            {
                if (parameter is OpenApiParameter { Name: { } name } param)
                {
                    param.Name = JsonNamingPolicy.CamelCase.ConvertName(name);
                }
            }
            return Task.CompletedTask;
        });
    });

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi()
       .WithDocumentPerVersion();

    app.MapScalarApiReference(options =>
    {
        var descriptions = app.DescribeApiVersions();

        var latest = descriptions[^1];

        foreach (var description in descriptions)
        {
            options.AddDocument(
                description.GroupName,
                description.GroupName,
                isDefault: description == latest);
        }
    });

    app.MapGet("/", () => Results.Redirect("/scalar"))
       .ExcludeFromDescription();
}

app.Run();

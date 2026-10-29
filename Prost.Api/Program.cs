using Asp.Versioning;
using FluentValidation;
using Microsoft.OpenApi;
using Prost.Api.Common.DependencyInjection;
using Prost.Api.Common.Diagnostics;
using Prost.Api.Common.Validation;
using Prost.Api.Data;
using Prost.Api.Data.Diagnostics;
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

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddServicesFromAssembly(
    typeof(Program).Assembly);

builder.Services.AddOptionsFromAssembly(
    typeof(Program).Assembly);

builder.Services.AddEndpointsFromAssembly(
    typeof(Program).Assembly);

builder.Services.AddEndpointConfigurationsFromAssembly(
    typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly,
    includeInternalTypes: true);

builder.Services.AddMediator(options =>
{
    options.Assemblies = [typeof(Program).Assembly];
    options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.GenerateTypesAsInternal = true;
});

builder.Services.AddData(builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddSelfHealthCheck()
    .AddDataHealthChecks(builder.Configuration);

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

app.MapSelfHealthCheck()
   .MapDataHealthChecks();

app.MapEndpointConfigurations(
    app.NewVersionedApi()
       .MapGroup("api/v{version:apiVersion}"));

app.Run();

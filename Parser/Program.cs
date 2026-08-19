using System.Text.Json.Serialization;

using Parser.Models;
using Parser.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddScoped<IParserService, ParserService>();

var app = builder.Build();

app.UseStatusCodePages(); 
app.UseExceptionHandler(); 
app.UseHttpsRedirection();

app.MapPost("/api/v1/parse-content", (ParseRequest request, IParserService parser) =>
{  
    var result = parser.Parse(request);

    return result.Success
        ? Results.Ok(new { count = result.Value.Count, records = result.Value })
        : Results.Problem(title: "Invalid request", detail: result.Error.Description, statusCode: 400);
})
.Accepts<ParseRequest>("application/json");

app.Run();

// expose the Program class for testing
public partial class Program { }
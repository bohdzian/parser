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
    ParseResult result = parser.Parse(request);
    
    return result.Success
        ? Results.Ok(new {
            count = result.Data.Count,
            records = result.Data
        })
        : Results.Problem(
            title: "Invalid request",
            detail: result.Error,
            statusCode: 400
        );
})
.Accepts<ParseRequest>("application/json");

app.Run();

//public class ParsedRecord
//{
//    public Dictionary<string, string> Fields { get; set; }
//}
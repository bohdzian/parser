using CsvHelper;
using System.Globalization;
using System.Text;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseStatusCodePages(); 
app.UseExceptionHandler(); 


app.UseHttpsRedirection();

app.MapPost("/api/v1/parse-content", (ParseRequest request) =>
{  
    if (!(request.Type == "CSV" || request.Type == "INTERNAL_JSON"))
    {
        return Results.BadRequest(new { Error = "Incorrect type" });
    } 
    
    byte[] content;
    string decodedString;
    
    try
    {
        content = Convert.FromBase64String(request.Content);
        decodedString = Encoding.UTF8.GetString(content);
    }
    catch (FormatException)
    {
        return Results.BadRequest(new { Error = "Content is not valid Base64" });
    }
    
    var records = new List<object>();
       
    try
    {   
        switch (request.Type)
        {
            case "CSV":
            {
                using var reader = new StringReader(decodedString);
                var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                records = csv.GetRecords<object>().ToList();
                break;
            }
            case "INTERNAL_JSON":
            {
                records = JsonSerializer.Deserialize<List<object>>(decodedString);
                break;
            }
        }
    }
    catch (JsonException)
    {
        return Results.BadRequest(new { Error = "Invalid JSON content" });
    }
    catch (CsvHelperException)
    {
        return Results.BadRequest(new { Error = "Invalid CSV content" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = "Unable to parse content", Detail = ex.Message });
    }

    return Results.Ok(new { count = records.Count(), records = records });
})
.Accepts<ParseRequest>("application/json");

app.Run();

record ParseRequest(string Type, string Content) {}
using CsvHelper;
using System.Globalization;
using System.Text;
using System.Text.Json;

using Parser.Models;

namespace Parser.Services;

public class ParserService : IParserService 
{
	private readonly int _maxContentSize;
	
	public ParserService(IConfiguration configuration)
	{
		_maxContentSize = configuration.GetValue<int>("Parser:MaxContentSizeMB", 10) * 1024 * 1024;
	}
	
	public ParseResult Parse(ParseRequest request) 
	{
		if (request.Content.Length > _maxContentSize)
			return ParseResult.Fail($"Content exceeds {_maxContentSize / 1024 / 1024} MB limit");
			
		if (!Enum.IsDefined(request.Type))
        	return ParseResult.Fail("Invalid type. Allowed: CSV, INTERNAL_JSON");
			
	    byte[] content;
	    string decodedString;
	    
	    try
	    {
	        //using var memoryStream = new MemoryStream(Convert.FromBase64String(request.Content));
	        //decodedString = Encoding.UTF8.GetString(memoryStream);
	        content = Convert.FromBase64String(request.Content);
    		using var memoryStream = new MemoryStream(content);
    		using var reader = new StreamReader(memoryStream, Encoding.UTF8);
    		decodedString = reader.ReadToEnd();
	    }
	    catch (FormatException)
	    {
	        return ParseResult.Fail("Content is not valid Base64");
	    }
	    catch
        {
            return ParseResult.Fail("Failed to decode content");
        }
	    
	    var records = new List<object>();
	    
	    try
	    {   
	    	records = request.Type switch
			{
			    ContentType.CSV => ParseCsv(decodedString),
			    ContentType.INTERNAL_JSON => JsonSerializer.Deserialize<List<object>>(decodedString),
			};
	    }
	    catch (JsonException)
	    {
	        return ParseResult.Fail("Invalid JSON content");
	    }
	    catch (CsvHelperException ex)
	    {
	        return ParseResult.Fail($"Invalid CSV content. {ex.Message}");
	    }
	    catch (Exception ex)
	    {
	        return ParseResult.Fail($"Parsing error: {ex.Message}");
	    }
	
	    return ParseResult.Ok(records);
	}
	
	private List<object> ParseCsv(string decodedString)
	{
		using var reader = new StringReader(decodedString);
		var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
		return csv.GetRecords<object>().ToList();
	}
	
	//private List<Dictionary<string, object>> ParseCsv(string decodedString)
	//{
		//return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(decodedString)
	//}
}

using System.Text;

using Parser.Models;

namespace Parser.Services;

public class ParserService : IParserService 
{
	private readonly int _maxContentSize;
	
	public ParserService(IConfiguration configuration)
	{
		_maxContentSize = configuration.GetValue<int>("Parser:MaxContentSizeMB", 10) * 1024 * 1024;
	}
	
	public ServiceParseResult Parse(ParseRequest request) 
	{
		if (request.Content.Length > _maxContentSize)
			return ServiceParseResult.Fail($"Content exceeds {_maxContentSize / 1024 / 1024} MB limit");
			
		if (!Enum.IsDefined(request.Type))
        	return ServiceParseResult.Fail("Invalid type. Allowed: CSV, INTERNAL_JSON");
			
	    var decodeResult = ConvertFromBase64(request.Content);
	    
	    if (!decodeResult.Success)
	    	ServiceParseResult.Fail(decodeResult.Error);
	    	
	    string decodedString = decodeResult.Data;
	    	
		var parserHandler = GetParserHandler(request.Type);
		var parseResult = parserHandler.Handle(decodedString);
		if (!parseResult.Success)
	    	return ServiceParseResult.Fail(parseResult.Error);

	    return ServiceParseResult.Ok(parseResult.Data);
	}
	
	private ParseResult<string> ConvertFromBase64(string content)
	{
		try
	    {
	        byte[] c = Convert.FromBase64String(content);
    		using var memoryStream = new MemoryStream(c);
    		using var reader = new StreamReader(memoryStream, Encoding.UTF8);
    		
    		return ParseResult<string>.Ok(reader.ReadToEnd());
	    }
	    catch (FormatException)
	    {
	    	return ParseResult<string>.Fail("Content is not valid Base64");
	    }
	    catch
        {
        	return ParseResult<string>.Fail("Failed to decode content");
        }
	}
	
	private IParserHandler GetParserHandler(ContentType type)
	{
	    return type switch
	    {
	        ContentType.CSV => new ParserHandlerCSV(),
	        ContentType.INTERNAL_JSON => new ParserHandlerInternalJSON()
	    };
	}
}
using System.Text.Json;

namespace Parser.Services;

public class ParserHandlerInternalJSON : IParserHandler
{
	public ServiceParseResult Handle(string decodedString)
	{
		try
	    {
			return ServiceParseResult.Ok(JsonSerializer.Deserialize<List<object>>(decodedString));
		}
		catch (JsonException)
	    {
	        return ServiceParseResult.Fail("Invalid JSON content");
	    }
	}
}
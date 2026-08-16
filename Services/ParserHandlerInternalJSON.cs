using System.Text.Json;

namespace Parser.Services;

public class ParserHandlerInternalJSON : IParserHandler
{
	public ParseResult<List<object>> Handle(string decodedString)
	{
		try
	    {
			return ParseResult<List<object>>.Ok(JsonSerializer.Deserialize<List<object>>(decodedString));
		}
		catch (JsonException)
	    {
	        return ParseResult<List<object>>.Fail("Invalid JSON content");
	    }
	}
}
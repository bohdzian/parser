using System.Text.Json;

namespace Parser.Services;

public class ParserHandlerInternalJSON : IParserHandler
{
	public Result<List<object>> Handle(string decodedString)
	{
		try
	    {
			return JsonSerializer.Deserialize<List<object>>(decodedString);
		}
		catch (JsonException)
	    {
	        return Errors.InvalidJSON;
	    }
	}
}
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Parser.Services;

public class ParserHandlerInternalJson : IParserHandler
{
	public Result<List<object>> Handle(string decodedString)
	{
		try
	    {
	    	var node = JsonNode.Parse(decodedString);

            var array = node switch
            {
                JsonArray arr => arr,
                JsonObject obj => new JsonArray(obj.DeepClone())
            };

			return array.Deserialize<List<object>>();
		}
		catch (JsonException)
	    {
	        return Errors.InvalidJson;
	    }
	}
}
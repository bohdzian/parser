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
	
	public Result<List<object>> Parse(ParseRequest request)
	{	
		if (request.Content.Length > _maxContentSize)
			return Errors.PayloadTooLarge;
			
		if (!Enum.IsDefined(request.Type))
        	return Errors.ContentTypeNotAllowed;
			
	    var decodeResult = ConvertFromBase64(request.Content);

	    if (!decodeResult.Success)
	    	return decodeResult.Error;
	    	
	    string decodedString = decodeResult.Value;
	    	
		var parserHandler = GetParserHandler(request.Type);
		var parseResult = parserHandler.Handle(decodedString);

		if (!parseResult.Success)
	    	return parseResult.Error;

	    return parseResult.Value;
	}
	
	private Result<string> ConvertFromBase64(string content)
	{
		try
	    {
	        byte[] c = Convert.FromBase64String(content);
    		using var memoryStream = new MemoryStream(c);
    		using var reader = new StreamReader(memoryStream, Encoding.UTF8);
    		var r = reader.ReadToEnd();
    		return new Result<string>(r);
	    }
	    catch (FormatException)
	    {
	    	return Errors.InvalidBase64;
	    }
	    catch
        {
        	return Errors.DecodingFailed;
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
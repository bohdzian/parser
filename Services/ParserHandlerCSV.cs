using System.Globalization;
using CsvHelper;

namespace Parser.Services;

public class ParserHandlerCSV : IParserHandler
{
	public ServiceParseResult Handle(string decodedString)
	{
		try
	    {
			using var reader = new StringReader(decodedString);
			var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			return ServiceParseResult.Ok(csv.GetRecords<object>().ToList());
		}
		catch (CsvHelperException ex)
	    {
	    	return ServiceParseResult.Fail($"Invalid CSV content. {ex.Message}");
	    }
	    catch (Exception ex)
	    {
	    	return ServiceParseResult.Fail($"Parsing error: {ex.Message}");
	    }
	}
}
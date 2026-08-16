using System.Globalization;
using CsvHelper;

namespace Parser.Services;

public class ParserHandlerCSV : IParserHandler
{
	public ParseResult<List<object>> Handle(string decodedString)
	{
		try
	    {
			using var reader = new StringReader(decodedString);
			var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			return ParseResult<List<object>>.Ok(csv.GetRecords<object>().ToList());
		}
		catch (CsvHelperException ex)
	    {
	    	return ParseResult<List<object>>.Fail($"Invalid CSV content. {ex.Message}");
	    }
	    catch (Exception ex)
	    {
	    	return ParseResult<List<object>>.Fail($"Parsing error: {ex.Message}");
	    }
	}
}
using System.Globalization;
using CsvHelper;

namespace Parser.Services;

public class ParserHandlerCSV : IParserHandler
{
	public Result<List<object>> Handle(string decodedString)
	{
		try
	    {
			using var reader = new StringReader(decodedString);
			var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			return csv.GetRecords<object>().ToList();
		}
		catch (CsvHelperException ex)
	    {
	    	return Errors.InvalidCSV;
	    }
	    catch (Exception ex)
	    {
	    	return Errors.ParsingFailed;
	    }
	}
}
using Parser.Models;

namespace Parser.Services;

public interface IParserService
{
	public ParseResult<List<object>> Parse(ParseRequest request);
}
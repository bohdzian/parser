using Parser.Models;

namespace Parser.Services;

public interface IParserService
{
	public ServiceParseResult Parse(ParseRequest request);
}
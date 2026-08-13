using Parser.Models;

namespace Parser.Services;

public interface IParserService
{
	ParseResult Parse(ParseRequest request);
}
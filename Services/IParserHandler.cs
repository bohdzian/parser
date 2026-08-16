namespace Parser.Services;

public interface IParserHandler
{
	public ParseResult<List<object>> Handle(string decodedString);
}
namespace Parser.Services;

public interface IParserHandler
{
	public ServiceParseResult Handle(string decodedString);
}
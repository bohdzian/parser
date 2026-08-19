namespace Parser.Services;

public interface IParserHandler
{
	public Result<List<object>> Handle(string decodedString);
}
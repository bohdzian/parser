using Parser.Models;

namespace Parser.Services;

public interface IParserService
{
	public Result<List<object>> Parse(ParseRequest request);
}
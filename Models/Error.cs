public enum ErrorType 
{ 
	PayloadTooLarge,
	ContentTypeNotAllowed,
	DecodingFailed,
	ParsingFailed,
	InvalidBase64,
	InvalidCSV,
	InvalidJSON,
}

public record Error(string Id, ErrorType Type, string Description);

public static class Errors
{
	public static Error PayloadTooLarge { get; } = new("PayloadTooLarge", ErrorType.PayloadTooLarge, "Content exceeds limit.");
	public static Error ContentTypeNotAllowed { get; } = new("ContentTypeNotAllowed", ErrorType.ContentTypeNotAllowed, "Invalid type. Allowed: CSV, INTERNAL_JSON.");
	public static Error DecodingFailed { get; } = new("DecodingFailed", ErrorType.DecodingFailed, "Failed to decode content.");
	public static Error ParsingFailed { get; } = new("ParsingFailed", ErrorType.ParsingFailed, "Failed to parse content.");
	public static Error InvalidBase64 { get; } = new("InvalidBase64", ErrorType.InvalidBase64, "Invalid Base64 content.");
	public static Error InvalidCSV { get; } = new("InvalidCSV", ErrorType.InvalidCSV, "Invalid CSV content.");
	public static Error InvalidJSON { get; } = new("InvalidJSON", ErrorType.InvalidJSON, "Invalid JSON content.");
}
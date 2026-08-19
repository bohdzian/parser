using System.ComponentModel.DataAnnotations;

namespace Parser.Models;

public record ParseRequest(
    [Required] ContentType Type,
    [Required] string Content
);

public enum ContentType
{
    CSV,
    INTERNAL_JSON
}
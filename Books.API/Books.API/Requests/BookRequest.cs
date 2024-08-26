using Books.Models;

namespace Books.API.Requests
{
    public record BookRequest(string bookName,string description, string publisherName, List<string> authorsName, int yearRelease);
    public record BookRequestUpdate(string? bookName, string? description, string? publisherName, List<string>? authorsName, int? yearRelease, int id);
}

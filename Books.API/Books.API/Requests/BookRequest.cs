using Books.Models;

namespace Books.API.Requests
{
    public record BookRequest(string bookName,string description, string publisherName, List<string> authorsName, int yearRelease);
    // Fazer um record BookResponse, trazendo os dados mais simples de book
    public class ConvertBook
    {
        public static Book RequestToEntity(BookRequest request)
        {
            Book book = new()
            {
                Name = request.bookName,
                YearCreated = request.yearRelease,
                Description = request.description,
                Authors = new List<Author>(),
                Publisher = new()
                {
                    Name = request.publisherName,
                }
            };
            foreach (string author in request.authorsName)
            {
                book.Authors.Add(new Author()
                {
                    Name = author,
                });
            }
            return book;
        }
    }
}

using Books.API.Requests;
using Books.API.Response;
using Books.Models;

namespace Books.API
{
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

        public static BookResponse EntityToResponse(Book entity)
        {
            BookResponse book = new()
            {
                Name = entity.Name,
                Id = entity.Id,
                Description = entity.Description,
                Publisher = entity.Publisher.Name,
                Authors = new List<string>()
            };
            foreach (var autor in entity.Authors)
            {
                book.Authors.Add(autor.Name);
            }
            return book;
        }
        public static List<BookResponse> EntitiesToResponse(List<Book> entities)
        {
            List<BookResponse> books = new();

            foreach (var book in entities)
            {
                BookResponse bookResponse = new()
                {
                    Name = book.Name,
                    Id = book.Id,
                    Description = book.Description,
                    Publisher = book.Publisher.Name,
                    Authors = new List<string>()
                };
                foreach (var author in book.Authors)
                {
                    bookResponse.Authors.Add(author.Name);
                }
                books.Add(bookResponse);
            }
            return books;
        }
    }
}

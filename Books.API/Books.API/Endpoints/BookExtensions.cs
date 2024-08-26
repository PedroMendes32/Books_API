using Books.API.Requests;
using Books.Database;
using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Books.API.Endpoints
{
    public static class BookExtensions
    {
        public static void AddBookEndpoins(this WebApplication app)
        {
            app.MapGet("/Book", ([FromServices]DataAccessLayer<Book> dataAccessLayer) =>
            {
                List<Book> listBooks = dataAccessLayer.GetAll().ToList();
                return listBooks != null ? Results.Ok(ConvertBook.EntitiesToResponse(listBooks)) : Results.NotFound();
            });

            app.MapGet("/Book/ById/{id}", ([FromServices]DataAccessLayer<Book> dataAccessLayer,int id) => 
            {
                Book? book = dataAccessLayer.GetEntityByFilter(x => x.Id == id);
                return book != null ? Results.Ok(ConvertBook.EntityToResponse(book)) : Results.NotFound();    
            });

            app.MapGet("/Book/ByAuthor/{author}", ([FromServices] DataAccessLayer<Book> dataAccessLayer, string author) => 
            {
                var books = dataAccessLayer.GetAll().ToList();
                if (!books.Any()) return Results.NotFound();
                List<Book> booksAuthor = new();

                booksAuthor = books.Where(x => x.Authors != null && x.Authors.Any(y => y.Name.ToUpper() == author.ToUpper())).ToList();

                return Results.Ok(ConvertBook.EntitiesToResponse(booksAuthor));
            });

            app.MapGet("/Book/ByPublisher/{Publisher}", ([FromServices] DataAccessLayer<Book> dataAccessLayer, string publisher) =>
            {
                var books = dataAccessLayer.GetAll().ToList();
                if (!books.Any()) return Results.NotFound();
                List<Book> booksPublisher = new();

                booksPublisher = books.Where(x => x.Publisher != null && x.Publisher.Name.ToUpper() == publisher.ToUpper()).ToList();
                return Results.Ok(ConvertBook.EntitiesToResponse(booksPublisher));
            });
            
            app.MapPost("/Book", ([FromServices]DataAccessLayer<Book> bookDAL,[FromServices]DataAccessLayer<Author> authorDAL,[FromServices]DataAccessLayer<Publisher> publisherDAL,[FromBody]BookRequest book) =>
            {
                if (bookDAL.GetEntityByFilter(x => x.Name.ToUpper() == book.bookName.ToUpper()) != null)
                {
                    return Results.Conflict();
                }

                var bookEntity = ConvertBook.RequestToEntity(book);

                List<Author> authorsInDatabase = new();
                foreach (var author in book.authorsName)
                {
                    Author? authorObj = authorDAL.GetEntityByFilter(x => x.Name.ToUpper() == author.ToUpper());
                    if (authorObj != null)
                    {
                        authorsInDatabase.Add(authorObj);
                    }
                    else 
                    {
                        authorsInDatabase.Add(new Author() { Name = author});
                    }
                }
                bookEntity.Authors = authorsInDatabase;

                Publisher? publisher = publisherDAL.GetEntityByFilter(x => x.Name.ToUpper() == book.publisherName.ToUpper());
                if (publisher != null)
                {
                    bookEntity.Publisher = publisher;
                }
                else
                {
                    bookEntity.Publisher = new Publisher() { Name = book.publisherName };
                }

                bookDAL.Add(bookEntity);
                return Results.Ok();
            });

            app.MapPut("/Book", ([FromServices] DataAccessLayer<Book> bookDAL, [FromServices]DataAccessLayer<Publisher> publisherDAL, [FromServices]DataAccessLayer<Author> authorDAL, [FromBody] BookRequestUpdate request) =>
            {
                var entity = bookDAL.GetEntityByFilter(x => x.Id == request.id);
                if (entity == null) return Results.NotFound();

                if (request.bookName != null && request.bookName.ToUpper() != entity.Name.ToUpper())
                {
                    entity.Name = request.bookName; 
                }

                if (request.description != null && request.description.ToUpper() != entity.Description.ToUpper())
                {
                    entity.Description = request.description;
                }

                if (request.yearRelease != null && request.yearRelease != entity.YearCreated)
                {
                    entity.YearCreated = (int)request.yearRelease;
                }

                List<Author> authorsInDatabase = new();
                if (request.authorsName != null && request.authorsName.Count > 0)
                {
                    entity.Authors.Clear();
                    foreach (var author in request.authorsName)
                    {
                        Author? authorObj = authorDAL.GetEntityByFilter(x => x.Name.ToUpper() == author.ToUpper());

                        if (authorObj == null)
                        {
                            authorsInDatabase.Add(new Author()
                            {
                                Name = author
                            });
                        }
                        else
                        {
                            authorsInDatabase.Add(authorObj);
                        }
                    }
                    entity.Authors = authorsInDatabase;
                }
                if (request.publisherName != null)
                {
                    Publisher? publisher = publisherDAL.GetEntityByFilter(x => x.Name.Equals(request.publisherName));
                    if (publisher != null)
                    {
                        entity.Publisher = publisher;
                    }
                    else
                    {
                        entity.Publisher = new Publisher() { Name = request.publisherName };
                    }
                }
                
                bookDAL.Update(entity);

                return Results.Ok();
            });

            app.MapDelete("/Book/{id}", ([FromServices] DataAccessLayer<Book> dataAccessLayer, int id) =>
            {
                var entity = dataAccessLayer.GetEntityByFilter(x => x.Id == id);
                if (entity == null) return Results.NotFound();

                dataAccessLayer.Remove(entity);
                return Results.Ok();
            });
        }
    }
}

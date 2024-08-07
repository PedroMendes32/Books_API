using Books.Database;
using Books.Models;
using Microsoft.AspNetCore.Mvc;

namespace Books.API.Endpoints
{
    public static class BookExtensions
    {
        public static void AddBookEndpoins(this WebApplication app)
        {
            app.MapGet("/Book", ([FromServices]DataAccessLayer<Book> dataAccessLayer) =>
            {
                List<Book> listBooks = dataAccessLayer.GetAll().ToList();
                return listBooks != null ? Results.Ok(listBooks) : Results.NotFound();
            });

            app.MapGet("/Book/{id}", ([FromServices]DataAccessLayer<Book> dataAccessLayer,int id) => 
            {
                Book? book = dataAccessLayer.GetEntityByFilter(x => x.Id == id);
                return book != null ? Results.Ok(book) : Results.NotFound();    
            });

            app.MapGet("/Book/{author}", ([FromServices] DataAccessLayer<Book> dataAccessLayer, string author) => 
            {
                var books = dataAccessLayer.GetAll().ToList();
                if (!books.Any()) return Results.NotFound();
                List<Book> booksAuthor = new();

                booksAuthor = books.Where(x => x.Authors != null && x.Authors.Any(y => y.Name == author)).ToList();

                return Results.Ok(booksAuthor);
            });

            app.MapGet("/Book/{Publisher}", ([FromServices] DataAccessLayer<Book> dataAccessLayer, string publisher) =>
            {
                var books = dataAccessLayer.GetAll().ToList();
                if (!books.Any()) return Results.NotFound();
                List<Book> booksPublisher = new();

                booksPublisher = books.Where(x => x.Publisher != null && x.Publisher.Name == publisher).ToList();
                return Results.Ok(booksPublisher);
            });


            app.MapPost("/Book", ([FromServices]DataAccessLayer<Book> dataAccessLayer, [FromBody]Book book) =>
            {
                dataAccessLayer.Add(book);
                return Results.Ok();
            });

            app.MapPut("/Book", ([FromServices] DataAccessLayer<Book> dataAccessLayer, [FromBody] Book book) =>
            {
                var entity = dataAccessLayer.GetEntityByFilter(x => x.Id == book.Id);
                if (entity == null) return Results.NotFound();

                entity.Name = book.Name;
                entity.Description = book.Description;
                entity.Created = book.Created;
                entity.Title = book.Title;
                entity.Authors = book.Authors;
                entity.Publisher = book.Publisher;
                
                dataAccessLayer.Update(entity);

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

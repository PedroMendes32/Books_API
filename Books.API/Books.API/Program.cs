using Books.API.Endpoints;
using Books.Database;
using Books.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<LibraryContext>();
builder.Services.AddTransient<DataAccessLayer<Author>>();
builder.Services.AddTransient<DataAccessLayer<Book>>();
builder.Services.AddTransient<DataAccessLayer<Publisher>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddBookEndpoins();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();






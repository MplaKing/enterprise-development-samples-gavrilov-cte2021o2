using AutoMapper;
using BookStore.Application;
using BookStore.Application.Contracts;
using BookStore.Application.Contracts.Author;
using BookStore.Application.Contracts.Book;
using BookStore.Application.Contracts.BookAuthor;
using BookStore.Application.Services;
using BookStore.Domain.Model;
using BookStore.Domain.Services;
using BookStore.Infractructure.EfCore.Services;
using BookStore.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(BookDto).Assembly.GetName().Name}.xml"));
});

var mapperConfig = new MapperConfiguration(config => config.AddProfile(new AutoMapperProfile()));
IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IRepository<Book, int>, BookEfCoreRepository>();
builder.Services.AddTransient<IAuthorRepository, AuthorEfCoreRepository>();
builder.Services.AddTransient<IRepository<BookAuthor, int>, BookAuthorEfCoreRepository>();

builder.Services.AddScoped<ICrudService<BookDto, BookCreateUpdateDto, int>, BookCrudService>();
builder.Services.AddScoped<ICrudService<AuthorDto, AuthorCreateUpdateDto, int>, AuthorCrudService>();
builder.Services.AddScoped<ICrudService<BookAuthorDto, BookAuthorCreateUpdateDto, int>, BookAuthorCrudService>();
builder.Services.AddScoped<IAnalyticsService, AuthorCrudService>();

builder.Services.AddDbContextFactory<BookStoreDbContext>(options =>
    options.UseLazyLoadingProxies().UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

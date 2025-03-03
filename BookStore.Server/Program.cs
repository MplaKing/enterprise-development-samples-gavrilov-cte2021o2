using AutoMapper;
using BookStore.Application;
using BookStore.Application.Contracts.Author;
using BookStore.Application.Contracts.Book;
using BookStore.Application.Contracts.BookAuthor;
using BookStore.Application.Contracts;
using BookStore.Application.Services;
using BookStore.Domain.Services;
using BookStore.Domain.Model;
using BookStore.Domain.Services.InMemory;
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

builder.Services.AddSingleton<IRepository<Book, int>,BookInMemoryRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorInMemoryRepository>();
builder.Services.AddSingleton<IRepository<BookAuthor, int>, BookAuthorInMemoryRepository>();

builder.Services.AddScoped<ICrudService<BookDto, BookCreateUpdateDto, int>, BookCrudService>();
builder.Services.AddScoped<ICrudService<AuthorDto, AuthorCreateUpdateDto, int>, AuthorCrudService>();
builder.Services.AddScoped<ICrudService<BookAuthorDto, BookAuthorCreateUpdateDto, int>, BookAuthorCrudService>();
builder.Services.AddScoped<IAnalyticsService, AuthorCrudService>();

builder.Services.AddDbContextFactory<BookStoreDbContext>(options =>
    options.UseLazyLoadingProxies().UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))));

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => { policy.WithOrigins("http://localhost:5244"); policy.AllowAnyMethod(); policy.AllowAnyHeader(); }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();

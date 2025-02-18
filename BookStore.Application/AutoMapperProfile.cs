using AutoMapper;
using BookStore.Application.Contracts.Author;
using BookStore.Application.Contracts.Book;
using BookStore.Application.Contracts.BookAuthor;
using BookStore.Domain.Model;

namespace BookStore.Application;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorCreateUpdateDto, Author>();

        CreateMap<Book, BookDto>();
        CreateMap<BookCreateUpdateDto, Book>();

        CreateMap<BookAuthor, BookAuthorDto>();
        CreateMap<BookAuthorCreateUpdateDto, BookAuthor>();
    }
}

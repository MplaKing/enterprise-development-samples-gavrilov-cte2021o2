using AutoMapper;
using BookStore.Application.Contracts;
using BookStore.Application.Contracts.BookAuthor;
using BookStore.Domain.Model;
using BookStore.Domain.Services;

namespace BookStore.Application.Services;
/// <summary>
/// Служба слоя приложения для манипуляции над связью авторов и книг
/// </summary>
/// <param name="repository">Доменная служба для связей</param>
/// <param name="bookRepository">Доменная служба для книг</param>
/// <param name="authorRepository">Доменная служба для авторов</param>
/// <param name="mapper">Автомаппер</param>
public class BookAuthorCrudService(IRepository<BookAuthor, int> repository,
    IRepository<Book, int> bookRepository,
    IAuthorRepository authorRepository,
    IMapper mapper)
    : ICrudService<BookAuthorDto, BookAuthorCreateUpdateDto, int>
{
    public bool Create(BookAuthorCreateUpdateDto newDto)
    {
        try
        {
            var newBookAuthor = mapper.Map<BookAuthor>(newDto);
            newBookAuthor.Id = repository.GetAll().Max(x => x.Id) + 1;

            var author = authorRepository.Get(newBookAuthor.AuthorId);
            var book = bookRepository.Get(newBookAuthor.BookId);

            newBookAuthor.Author = author;
            newBookAuthor.Book = book;

            author?.BookAuthors?.Add(newBookAuthor);
            book?.BookAuthors?.Add(newBookAuthor);

            authorRepository.Update(author);
            bookRepository.Update(book);

            return repository.Add(newBookAuthor);
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            var bookAuthor = repository.Get(id);

            var author = authorRepository.Get(bookAuthor.AuthorId);
            var book = bookRepository.Get(bookAuthor.BookId);

            author?.BookAuthors?.Remove(bookAuthor);
            book?.BookAuthors?.Remove(bookAuthor);

            authorRepository.Update(author);
            bookRepository.Update(book);

            return repository.Delete(id);
        }
        catch
        {
            return false;
        }
    }

    public BookAuthorDto? GetById(int id)
    {
        var bookAuthor = repository.Get(id);
        return mapper.Map<BookAuthorDto>(bookAuthor);
    }

    public IList<BookAuthorDto> GetList() =>
        mapper.Map<List<BookAuthorDto>>(repository.GetAll());

    public bool Update(int key, BookAuthorCreateUpdateDto newDto)
    {
        try
        {
            var oldBookAuthor = repository.Get(key);
            var newBookAuthor = mapper.Map<BookAuthor>(newDto);
            newBookAuthor.Id = key;

            var oldAuthor = authorRepository.Get(oldBookAuthor.AuthorId);
            var oldBook = bookRepository.Get(oldBookAuthor.BookId);
            oldAuthor?.BookAuthors?.Remove(oldBookAuthor);
            oldBook?.BookAuthors?.Remove(oldBookAuthor);
            authorRepository.Update(oldAuthor);
            bookRepository.Update(oldBook);

            var newAuthor = authorRepository.Get(newBookAuthor.AuthorId);
            var newBook = bookRepository.Get(newBookAuthor.BookId);
            newAuthor?.BookAuthors?.Add(newBookAuthor);
            newBook?.BookAuthors?.Add(newBookAuthor);
            authorRepository.Update(newAuthor);
            bookRepository.Update(newBook);
            return repository.Update(newBookAuthor);
        }
        catch
        {
            return false;
        }
    }
}

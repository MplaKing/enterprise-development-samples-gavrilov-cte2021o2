using AutoMapper;
using BookStore.Application.Contracts;
using BookStore.Application.Contracts.Book;
using BookStore.Domain.Model;
using BookStore.Domain.Services;

namespace BookStore.Application.Services;
/// <summary>
/// Служба слоя приложения для манипуляции над книгами
/// </summary>
/// <param name="repository">Доменная служба для книг</param>
/// <param name="mapper">Автомаппер</param>
public class BookCrudService(IRepository<Book, int> repository, IMapper mapper) : ICrudService<BookDto, BookCreateUpdateDto, int>
{
    public bool Create(BookCreateUpdateDto newDto)
    {
        var newBook = mapper.Map<Book>(newDto);
        newBook.Id = repository.GetAll().Max(x => x.Id) + 1;
        var result = repository.Add(newBook);
        return result;
    }

    public bool Delete(int id) =>
        repository.Delete(id);

    public BookDto? GetById(int id)
    {
        var book = repository.Get(id);
        return mapper.Map<BookDto>(book);
    }

    public IList<BookDto> GetList() =>
        mapper.Map<List<BookDto>>(repository.GetAll());

    public bool Update(int key, BookCreateUpdateDto newDto)
    {
        var oldBook = repository.Get(key);
        var newBook = mapper.Map<Book>(newDto);
        newBook.Id = key;
        newBook.BookAuthors = oldBook?.BookAuthors;
        var result = repository.Update(newBook);
        return result;
    }
}

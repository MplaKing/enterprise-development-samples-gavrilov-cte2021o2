using BookStore.Domain.Data;
using BookStore.Domain.Model;

namespace BookStore.Domain.Services.InMemory;
/// <summary>
/// Имплементация репозитория для связующей сущности книг и авторов, которая хранит коллекцию в оперативной памяти 
/// </summary>
public class BookAuthorInMemoryRepository : IRepository<BookAuthor, int>
{
    private List<BookAuthor> _bookAuthors;
    public BookAuthorInMemoryRepository()
    {
        _bookAuthors = DataSeeder.BookAuthors;
    }
    public bool Add(BookAuthor entity)
    {
        try
        {
            _bookAuthors.Add(entity);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public bool Delete(int key)
    {
        try
        {
            var bookAuthor = Get(key);
            if (bookAuthor != null)
                _bookAuthors.Remove(bookAuthor);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public BookAuthor? Get(int key) =>
        _bookAuthors.FirstOrDefault(item => item.Id == key);

    public IList<BookAuthor> GetAll() =>
        _bookAuthors;

    public bool Update(BookAuthor entity)
    {
        try
        {
            Delete(entity.Id);
            Add(entity);
        }
        catch
        {
            return false;
        }
        return true;
    }
}

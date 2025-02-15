using BookStore.Domain.Data;
using BookStore.Domain.Model;

namespace BookStore.Domain.Services.InMemory;
/// <summary>
/// Имплементация репозитория для книг, которая хранит коллекцию в оперативной памяти 
/// </summary>
public class BookInMemoryRepository : IRepository<Book, int>
{
    private List<Book> _books;
    public BookInMemoryRepository()
    {
        _books = DataSeeder.Books;
    }

    public bool Add(Book entity)
    {
        try
        {
            _books.Add(entity);
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
            var book = Get(key);
            if (book != null)
                _books.Remove(book);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public Book? Get(int key) =>
        _books.FirstOrDefault(item => item.Id == key);

    public IList<Book> GetAll() =>
        _books;

    public bool Update(Book entity)
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

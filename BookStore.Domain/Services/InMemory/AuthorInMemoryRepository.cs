using BookStore.Domain.Data;
using BookStore.Domain.Model;

namespace BookStore.Domain.Services.InMemory;
/// <summary>
/// Имплементация репозитория для авторов, которая хранит коллекцию в оперативной памяти 
/// </summary>
public class AuthorInMemoryRepository : IAuthorRepository
{
    private List<Author> _authors;
    public AuthorInMemoryRepository()
    {
        _authors = DataSeeder.Authors;
    }

    public bool Add(Author entity)
    {
        try
        {
            _authors.Add(entity);
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
            var author = Get(key);
            if (author != null)
                _authors.Remove(author);
        }
        catch
        {
            return false;
        }
        return true;
    }
    public bool Update(Author entity)
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
    public Author? Get(int key) =>
        _authors.FirstOrDefault(item => item.Id == key);
    public IList<Author> GetAll() =>
        _authors;

    public IList<Tuple<string, int>> GetLast5AuthorsBook(int key)
    {
        var author = Get(key);
        var books = new List<Book>();
        if (author != null && author.BookAuthors?.Count > 0)
            foreach (var bs in author.BookAuthors)
                if (bs.Book != null)
                    books.Add(bs.Book);
        return books
            .OrderByDescending(book => book.Year)
            .Take(5)
            .Select(book => new Tuple<string, int>(book.ToString(), book.Year ?? 0))
            .ToList();
    }

    public IList<Tuple<string, int>> GetTop5AuthorsByPageCount() =>
        _authors
            .OrderByDescending(author => author.GetPageCount())
            .Take(5)
            .Select(author => new Tuple<string, int>(author.ToString(), author.GetPageCount() ?? 0))
            .ToList();

}

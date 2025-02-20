using BookStore.Domain.Data;
using BookStore.Domain.Model;

namespace BookStore.Domain.Services.InMemory;
/// <summary>
/// Имплементация репозитория для авторов, которая хранит коллекцию в оперативной памяти 
/// </summary>
public class AuthorInMemoryRepository : IAuthorRepository
{
    private List<Author> _authors;
    private List<Book> _books;
    private List<BookAuthor> _bookAuthors;
    /// <summary>
    /// Конструктор репозитория
    /// </summary>
    public AuthorInMemoryRepository()
    {
        _authors = DataSeeder.Authors;
        _books = DataSeeder.Books;
        _bookAuthors = DataSeeder.BookAuthors;

        foreach (var ba in _bookAuthors)
        {
            ba.Author = _authors.FirstOrDefault(a => a.Id == ba.AuthorId);
            ba.Book = _books.FirstOrDefault(a => a.Id == ba.BookId);
        }

        foreach (var b in _books)
        {
            b.BookAuthors = [];
            b.BookAuthors?.AddRange(_bookAuthors.Where(ba => ba.BookId == b.Id));
        }

        foreach (var a in _authors)
        {
            a.BookAuthors = [];
            a.BookAuthors?.AddRange(_bookAuthors.Where(ba => ba.AuthorId == a.Id));
        }
    }

    /// <inheritdoc/>
    public Task<Author> Add(Author entity)
    {
        try
        {
            _authors.Add(entity);
        }
        catch
        {
            return null!;
        }
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int key)
    {
        try
        {
            var author = await Get(key);
            if (author != null)
                _authors.Remove(author);
        }
        catch
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public async Task<Author> Update(Author entity)
    {
        try
        {
            await Delete(entity.Id);
            await Add(entity);
        }
        catch
        {
            return null!;
        }
        return entity;
    }

    /// <inheritdoc/>
    public Task<Author?> Get(int key) =>
        Task.FromResult(_authors.FirstOrDefault(item => item.Id == key));

    /// <inheritdoc/>
    public Task<IList<Author>> GetAll() =>
        Task.FromResult((IList<Author>)_authors);

    /// <inheritdoc/>
    public async Task<IList<Tuple<string, int>>> GetLast5AuthorsBook(int key)
    {
        var author = await Get(key);
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

    /// <inheritdoc/>
    public Task<IList<Tuple<string, int>>> GetTop5AuthorsByPageCount() =>
        Task.FromResult((IList<Tuple<string, int>>)_authors
            .OrderByDescending(author => author.GetPageCount())
            .Take(5)
            .Select(author => new Tuple<string, int>(author.ToString(), author.GetPageCount() ?? 0))
            .ToList());

}

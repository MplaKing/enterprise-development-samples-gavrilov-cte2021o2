using BookStore.Domain.Model;
using BookStore.Domain.Services;
using BookStore.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infractructure.EfCore.Services;
public class BookAuthorEfCoreRepository(BookStoreDbContext context) : IRepository<BookAuthor, int>
{
    private readonly DbSet<BookAuthor> _bookAuthors = context.BookAuthors;
    public async Task<BookAuthor> Add(BookAuthor entity)
    {
        var result = await _bookAuthors.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }
    
    public async Task<bool> Delete(int key)
    {
        var entity = await _bookAuthors.FirstOrDefaultAsync(e =>e.Id == key);
        if (entity == null)
            return false;
        _bookAuthors.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<BookAuthor?> Get(int key) =>
        await _bookAuthors.FirstOrDefaultAsync(e => e.Id == key);
    
    public async Task<IList<BookAuthor>> GetAll() =>
        await _bookAuthors.ToListAsync();

    public async Task<BookAuthor> Update(BookAuthor entity)
    {
        _bookAuthors.Update(entity);
        await context.SaveChangesAsync();
        return (await Get(entity.Id))!;
    }

}

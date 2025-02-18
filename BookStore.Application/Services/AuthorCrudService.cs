using AutoMapper;
using BookStore.Application.Contracts;
using BookStore.Application.Contracts.Author;
using BookStore.Domain.Model;
using BookStore.Domain.Services;

namespace BookStore.Application.Services;
/// <summary>
/// Служба слоя приложения для манипуляции над авторами
/// </summary>
/// <param name="repository">Доменная служба для авторов</param>
/// <param name="mapper">Автомаппер</param>
public class AuthorCrudService(IAuthorRepository repository, IMapper mapper) : ICrudService<AuthorDto, AuthorCreateUpdateDto, int>, IAnalyticsService
{
    public bool Create(AuthorCreateUpdateDto newDto)
    {
        var newAuthor = mapper.Map<Author>(newDto);
        newAuthor.Id = repository.GetAll().Max(x => x.Id) + 1;
        var result = repository.Add(newAuthor);
        return result;
    }

    public bool Delete(int id) =>
        repository.Delete(id);

    public AuthorDto? GetById(int id)
    {
        var author = repository.Get(id);
        return mapper.Map<AuthorDto>(author);
    }

    public IList<AuthorDto> GetList() =>
        mapper.Map<List<AuthorDto>>(repository.GetAll());

    public bool Update(int key, AuthorCreateUpdateDto newDto)
    {
        var oldAuthor = repository.Get(key);
        var newAuthor = mapper.Map<Author>(newDto);
        newAuthor.Id = key;
        newAuthor.BookAuthors = oldAuthor?.BookAuthors;
        var result = repository.Update(newAuthor);
        return result;
    }

    public IList<Tuple<string, int>> GetTop5AuthorsByPageCount() =>
        repository.GetTop5AuthorsByPageCount();

    public IList<Tuple<string, int>> GetLast5AuthorsBook(int key) =>
        repository.GetLast5AuthorsBook(key);
}

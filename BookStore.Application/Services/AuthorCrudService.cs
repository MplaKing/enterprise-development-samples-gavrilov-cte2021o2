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
    public async Task<AuthorDto> Create(AuthorCreateUpdateDto newDto)
    {
        var newAuthor = mapper.Map<Author>(newDto);
        var res = await repository.Add(newAuthor);
        return mapper.Map<AuthorDto>(res);
    }

    public async Task<bool> Delete(int id) =>
        await repository.Delete(id);

    public async Task<AuthorDto?> GetById(int id)
    {
        var author = await repository.Get(id);
        return mapper.Map<AuthorDto>(author);
    }

    public async Task<IList<AuthorDto>> GetList() =>
        mapper.Map<List<AuthorDto>>(await repository.GetAll());

    public async Task<AuthorDto> Update(int key, AuthorCreateUpdateDto newDto)
    {
        var newAuthor = mapper.Map<Author>(newDto);
        await repository.Update(newAuthor);
        return mapper.Map<AuthorDto>(newAuthor);
    }

    public async Task<IList<Tuple<string, int>>> GetTop5AuthorsByPageCount() =>
        await repository.GetTop5AuthorsByPageCount();

    public async Task<IList<Tuple<string, int>>> GetLast5AuthorsBook(int key) =>
        await repository.GetLast5AuthorsBook(key);
}

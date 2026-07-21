using webapi.Model;
using webapi.Repositories;

namespace webapi.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;

    public ItemService(IItemRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Item>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Item?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<Item> CreateAsync(Item item)
    {
        return _repository.AddAsync(item);
    }
}
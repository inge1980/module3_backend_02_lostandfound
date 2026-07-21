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

    public Task<IEnumerable<Item>> GetAllAsync(
        ItemStatus? status = null,
        string? category = null,
        string? q = null)
    {
        return _repository.GetAllAsync(status, category, q);
    }

    public Task<Item?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<Item> CreateAsync(Item item)
    {
        return _repository.AddAsync(item);
    }

    public Task DeleteAsync(Item item)
    {
        item.EnsureCanDelete();
        return _repository.DeleteAsync(item);
    }
}
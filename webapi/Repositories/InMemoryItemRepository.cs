using webapi.Model;

namespace webapi.Repositories;

public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> items = new();


    public Task<IEnumerable<Item>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Item>>(items);
    }


    public Task<Item?> GetByIdAsync(Guid id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);

        return Task.FromResult(item);
    }


    public Task<Item> AddAsync(Item item)
    {
        items.Add(item);

        return Task.FromResult(item);
    }


    public Task DeleteAsync(Item item)
    {
        items.Remove(item);

        return Task.CompletedTask;
    }
}
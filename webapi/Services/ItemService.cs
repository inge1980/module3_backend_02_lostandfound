using webapi.Model;

namespace webapi.Services;

public class ItemService : IItemService
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


    public Task<Item> CreateAsync(Item item)
    {
        items.Add(item);
        return Task.FromResult(item);
    }
}
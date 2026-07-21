using webapi.Model;

namespace webapi.Repositories;

public class InMemoryItemRepository : IItemRepository
{
    private readonly List<Item> items = new();


    public Task<IEnumerable<Item>> GetAllAsync(
        ItemStatus? status = null,
        string? category = null,
        string? q = null)
    {
        IEnumerable<Item> result = items;
        if (status.HasValue) {
            result = result.Where(x => x.Status == status);
        }
        if (!string.IsNullOrWhiteSpace(category)) {
            result = result.Where(x =>
                x.Category.Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(q)) {
            result = result.Where(x =>
                x.Title.Contains(q, StringComparison.OrdinalIgnoreCase)
                ||
                x.Description.Contains(q, StringComparison.OrdinalIgnoreCase));
        }
        return Task.FromResult(result);
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
        if(items.Contains(item)) {
            items.Remove(item);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Item item)
    {
        var existing = items.FirstOrDefault(i => i.Id == item.Id);

        if (existing != null)
        {
            items.Remove(existing);
            items.Add(item);
        }

        return Task.CompletedTask;
    }

}
using webapi.Model;

namespace webapi.Services;

public class ItemService : IItemService
{

    private readonly List<Item> items = new()
    {
        new Item
        {
            Title = "Found item 1",
            Description = "Found during cleaning up the park",
            Category = "Other",
            FoundLocation = "Park",
            ClaimedBy = string.Empty,
            ClaimedAtUtc = null,
            ReturnedAtUtc = null
        },
        new Item
        {
            Title = "Found item 2",
            Description = "Found during a busy day at the mall",
            Category = "Other",
            FoundLocation = "Park",
            ClaimedBy = "Jane Doe",
            ClaimedAtUtc = DateTime.UtcNow.AddDays(-3),
            ReturnedAtUtc = DateTime.UtcNow.AddDays(-1)
        },
        new Item
        {
            Title = "Found item 3",
            Description = "Found during a long road trip",
            Category = "Other",
            FoundLocation = "Park",
            ClaimedBy = "John Doe",
            ClaimedAtUtc = DateTime.UtcNow.AddDays(-1),
            ReturnedAtUtc = null
        }
    };


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
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
            FoundAtUtc = DateTime.UtcNow.AddDays(-5),
            Status = ItemStatus.Available,
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
            FoundAtUtc = DateTime.UtcNow.AddDays(-5),
            Status = ItemStatus.Returned,
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
            FoundAtUtc = DateTime.UtcNow.AddDays(-5),
            Status = ItemStatus.Claimed,
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
        item.FoundAtUtc = DateTime.UtcNow;
        items.Add(item);
        return Task.FromResult(item);
    }
}
using webapi.Model;

namespace webapi.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllAsync(
        ItemStatus? status = null,
        string? category = null,
        string? q = null
    );

    Task<Item?> GetByIdAsync(Guid id);

    Task<Item> CreateAsync(Item item);

    Task DeleteAsync(Item item);

    Task UpdateAsync(Item item);
}
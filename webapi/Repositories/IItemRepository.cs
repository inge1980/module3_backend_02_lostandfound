using webapi.Model;

namespace webapi.Repositories;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync(
        ItemStatus? status = null,
        string? category = null,
        string? q = null
    );

    Task<Item?> GetByIdAsync(Guid id);

    Task<Item> AddAsync(Item item);

    Task DeleteAsync(Item item);
}
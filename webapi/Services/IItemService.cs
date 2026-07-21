using webapi.Model;

namespace webapi.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllAsync();

    Task<Item?> GetByIdAsync(Guid id);

    Task<Item> CreateAsync(Item item);

    Task DeleteAsync(Item item);
}
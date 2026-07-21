using webapi.Model;

namespace webapi.Services;

public interface IItemService
{
    Task<IEnumerable<Item>> GetAllAsync();

    Task<Item?> GetByIdAsync(int id);

    Task<Item> CreateAsync(Item item);
}
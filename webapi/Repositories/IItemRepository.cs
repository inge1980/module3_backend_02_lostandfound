using webapi.Model;

namespace webapi.Repositories;

public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllAsync();

    Task<Item?> GetByIdAsync(Guid id);

    Task<Item> AddAsync(Item item);

    Task DeleteAsync(Item item);
}
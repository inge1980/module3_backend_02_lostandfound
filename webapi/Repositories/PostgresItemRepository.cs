using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Model;

namespace webapi.Repositories;

public class PostgresItemRepository : IItemRepository
{
    private readonly ItemDbContext _context;

    public PostgresItemRepository(ItemDbContext context)
    {
        _context = context;
    }

    public async Task<Item> AddAsync(Item item)
    {
        _context.Items.Add(item);

        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<IEnumerable<Item>> GetAllAsync(
        ItemStatus? status,
        string? category,
        string? query)
    {
        IQueryable<Item> items = _context.Items;

        if (status.HasValue)
        {
            items = items.Where(i => i.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            items = items.Where(i => i.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(query))
        {
            items = items.Where(i =>
                i.Title.Contains(query) ||
                i.Description.Contains(query));
        }

        return await items.ToListAsync();
    }

    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task DeleteAsync(Item item)
    {
        _context.Items.Remove(item);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        _context.Items.Update(item);

        await _context.SaveChangesAsync();
    }
}
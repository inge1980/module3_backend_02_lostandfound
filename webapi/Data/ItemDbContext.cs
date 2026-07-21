using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data;

public class ItemDbContext : DbContext
{
    public ItemDbContext(DbContextOptions<ItemDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
}
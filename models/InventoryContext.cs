using Microsoft.EntityFrameworkCore;

namespace Argus.InventoryService.Models;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) 
    {

    }

    public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
}
using Dima.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Data;

public class AppDbContext : DbContext
{

    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration();
    }

}

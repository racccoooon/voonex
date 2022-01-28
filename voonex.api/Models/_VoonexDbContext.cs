using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace voonex.api.Models;

public class VoonexDbContext : DbContext
{
    public VoonexDbContext(DbContextOptions<VoonexDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoonexDbContext).Assembly);
        modelBuilder.Entity<User>();
        modelBuilder.Entity<Server>();
        modelBuilder.Entity<ServerMember>();
        modelBuilder.Entity<ChatMessage>();
    }
}
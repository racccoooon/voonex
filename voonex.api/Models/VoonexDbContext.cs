using Microsoft.EntityFrameworkCore;

namespace voonex.api.Models;

public class VoonexDbContext : DbContext
{
    public VoonexDbContext(DbContextOptions<VoonexDbContext> options)
        : base(options)
    {
        
    }
}
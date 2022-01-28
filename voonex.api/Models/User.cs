using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace voonex.api.Models;

public class User
{
    public Guid Id { get; set; }
    [StringLength(32)]
    public string Name { get; set; } = null!;
    [StringLength(64)]
    public string Password { get; set; } = null!;
    [StringLength(64)]
    public string Salt { get; set; } = null!;
    [StringLength(200)]
    public string Email { get; set; } = null!;
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
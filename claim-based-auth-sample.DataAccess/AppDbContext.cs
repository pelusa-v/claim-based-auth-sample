using claim_based_auth_sample.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace claim_based_auth_sample.DataAccess;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Note> Notes { get; set; }
}

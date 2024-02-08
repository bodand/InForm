using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext(
    DbContextOptions<InFormDbContext> ops
) : DbContext(ops)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("InForm");
        ConfigureModels(modelBuilder);
    }
}
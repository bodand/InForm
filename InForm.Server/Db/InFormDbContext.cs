using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

/// <summary>
///     The database connection object used by InForm.Server as a DAL.
/// </summary>
/// <param name="ops">The connection configuration options.</param>
public partial class InFormDbContext(
    DbContextOptions<InFormDbContext> ops
) : DbContext(ops)
{
    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("InForm");
        ConfigureModels(modelBuilder);
    }
}
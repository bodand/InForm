using InForm.Server.Features.FillForms.Db;

using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext
{
    [ModelConfiguration]
    private static void ConfigureFormFills(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FillData>()
            .UseTpcMappingStrategy();
    }

#nullable disable
    public DbSet<FillData> FillData { get; set; }

    public DbSet<StringFillData> StringFillData { get; set; }
    public DbSet<NumericRangeFillData> NumericRangeFillData { get; set; }
}
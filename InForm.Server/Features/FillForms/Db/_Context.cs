using InForm.Server.Features.FillForms.Db;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext {
    [ModelConfiguration]
    private static void ConfigureFormFills(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FillData>()
                    .UseTpcMappingStrategy();
        modelBuilder.Entity<MultiChoiceFillData>()
                    .HasMany(x => x.Selected)
                    .WithOne(x => x.FillData)
                    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MultiChoiceFillSelection>()
                    .HasOne(x => x.Option)
                    .WithMany();
    }

#nullable disable
    public DbSet<FillData> FillData { get; set; }

    public DbSet<StringFillData> StringFillData { get; set; }

    public DbSet<MultiChoiceFillData> MultiChoiceFillDatas { get; set; }
    public DbSet<MultiChoiceFillSelection> MultiChoiceFillSelections { get; set; }
}

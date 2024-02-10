using InForm.Features.Forms.Db;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext
{
    [ModelConfiguration]
    private static void ConfigureFormElements(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>()
            .HasMany(x => x.FormElementBases)
            .WithOne(x => x.ParentForm)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FormElementBase>()
            .UseTphMappingStrategy();
        modelBuilder.Entity<FillData>()
            .UseTpcMappingStrategy();

        modelBuilder.Entity<StringFormElement>()
            .HasMany(x => x.FillData)
            .WithOne(x => x.ParentElement)
            .OnDelete(DeleteBehavior.Cascade);
    }

#nullable disable
    public DbSet<Form> Forms { get; set; }
    
    public DbSet<FormElementBase> FormElementBases { get; set; }
    public DbSet<FillData> FillData { get; set; }

    public DbSet<StringFormElement> StringFormElements { get; set; }
    public DbSet<StringFillData> StringFillData { get; set; }
}
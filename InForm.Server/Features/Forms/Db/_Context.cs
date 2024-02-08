using InForm.Features.Forms.Db;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext
{
    [ModelConfiguration]
    private static void ConfigureFormElements(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>()
            .HasMany<FormElementBase>(x => x.FormElementBases)
            .WithOne(x => x.ParentForm)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FormElementBase>()
            .UseTphMappingStrategy();
    }

#nullable disable
    public DbSet<Form> Forms { get; set; }
    public DbSet<FormElementBase> FormElementBases { get; set; }
    public DbSet<RangeFormElement> RangeFormElements { get; set; }
    public DbSet<StringFormElement> StringFormElements { get; set; }
}
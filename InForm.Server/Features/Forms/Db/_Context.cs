using InForm.Features.Forms.Db;
using InForm.Server.Features.FillForms.Db;
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

        modelBuilder.Entity<StringFormElement>()
            .HasMany(x => x.FillData)
            .WithOne(x => x.ParentElement)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public async Task<IEnumerable<FormElementBase>> LoadAllElementsForForm(Form form)
    {
        var strings = await StringFormElements.Where(x => x.ParentForm == form).ToListAsync();
        return [
            .. strings
        ];
    }

#nullable disable
    public DbSet<Form> Forms { get; set; }

    public DbSet<FormElementBase> FormElementBases { get; set; }

    public DbSet<StringFormElement> StringFormElements { get; set; }
}
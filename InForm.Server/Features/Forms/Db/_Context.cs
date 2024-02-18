using InForm.Server.Features.FillForms.Db;
using InForm.Server.Features.Forms.Db;
using InForm.Server.Migrations;
using Microsoft.EntityFrameworkCore;

namespace InForm.Server.Db;

public partial class InFormDbContext {
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
        modelBuilder.Entity<MultiChoiceFormElement>()
                    .HasMany(x => x.FillData)
                    .WithOne(x => x.ParentElement)
                    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MultiChoiceFormElement>()
                    .Navigation(x => x.Options)
                    .AutoInclude();
    }

    public async Task<IEnumerable<FormElementBase>> LoadAllElementsForForm(Form form)
    {
        var strings = await SelectFormElementsIn(form, StringFormElements);
        var multis = await SelectFormElementsIn(form, MultiChoiceFormElements);
        return
        [
            .. strings,
            .. multis
        ];
    }

    public async Task<IEnumerable<FormElementBase>> LoadAllElementsForFormWithData(Form form)
    {
        var strings = await SelectFormElementsIn(form,
                                                 StringFormElements.Include(x => x.FillData));
        var multis = await SelectFormElementsIn(form,
                                                MultiChoiceFormElements.Include(x => x.FillData));
        return
        [
            .. strings,
            .. multis
        ];
    }

    private async Task<IEnumerable<TElement>> SelectFormElementsIn<TElement>(Form form, IQueryable<TElement> source)
        where TElement : FormElementBase
        => await source.Where(x => x.ParentFormId == form.Id).ToListAsync();

#nullable disable
    public DbSet<Form> Forms { get; set; }

    public DbSet<FormElementBase> FormElementBases { get; set; }

    public DbSet<StringFormElement> StringFormElements { get; set; }
    public DbSet<MultiChoiceFormElement> MultiChoiceFormElements { get; set; }
    public DbSet<MultiChoiceOption> MultiChoiceOptions { get; set; }
}

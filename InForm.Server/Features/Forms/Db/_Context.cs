using InForm.Server.Features.Forms.Db;
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

    internal async Task<IEnumerable<FormElementBase>> LoadAllElementsForForm(Form form)
    {
        var strings = await SelectFormElementsIn(form, StringFormElements);
        var multis = await SelectFormElementsIn(form, MultiChoiceFormElements);
        return
        [
            .. strings,
            .. multis
        ];
    }

    internal async Task<IEnumerable<FormElementBase>> LoadAllElementsForFormWithData(Form form)
    {
        var strings = await SelectFormElementsIn(form,
                                                 StringFormElements.Include(x => x.FillData));
        var multis = await SelectFormElementsIn(form,
                                                MultiChoiceFormElements
                                                    .Include(x => x.FillData)
                                                    .ThenInclude(x => x.Selected)
                                                    .ThenInclude(x => x.Option));
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
    /// <summary>
    ///     The db set for the form entities.
    /// </summary>
    public virtual DbSet<Form> Forms { get; set; }

    /// <summary>
    ///     The db set for the base form element entities.
    /// </summary>
    public virtual DbSet<FormElementBase> FormElementBases { get; set; }

    /// <summary>
    ///     The db set for the string form element entities.
    /// </summary>
    public virtual DbSet<StringFormElement> StringFormElements { get; set; }

    /// <summary>
    ///     The db set for the multi-choice form element entities.
    /// </summary>
    public virtual DbSet<MultiChoiceFormElement> MultiChoiceFormElements { get; set; }

    /// <summary>
    ///     The db set for the multi-choice option entities.
    /// </summary>
    public virtual DbSet<MultiChoiceOption> MultiChoiceOptions { get; set; }
}

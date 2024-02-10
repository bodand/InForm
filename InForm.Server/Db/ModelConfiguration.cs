namespace InForm.Server.Db;

/// <summary>
///     Attribute for annotating that a given method is to be used
///     in the configuration of the DB model.
///     A Roslyn source generator will generate a method that calls 
///     all methods with this attribute.
///     This allows the database to be configured separately, in 
///     each feature's directory, and removing the gigantic monolithic
///     DbContext class.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class ModelConfigurationAttribute : Attribute;

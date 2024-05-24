namespace InForm.Server;

/// <summary>
///     Configuration class representing the servers CORS configuration.
/// </summary>
public class CorsConfig {
    /// <summary>
    ///     Whether the server allows any CORS request, useful for debugging
    ///     and development. This value overrides all other CORS configs.
    /// </summary>
    public bool Any { get; set; } = false;

    /// <summary>
    ///     The list of origins to allow CORS from. By default, the empty list.
    /// </summary>
    /// <remarks>
    ///     This list contains the list of all valid sites to accept a CORS
    ///     request from. By default, it is empty, which forbids them from any
    ///     host.
    ///     This value is ignored if <see cref="Any"/> is set to true.
    /// </remarks>
    /// <seealso cref="Any"/>
    public string[] Origins { get; set; } = [];
}

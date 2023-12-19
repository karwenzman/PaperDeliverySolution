namespace PaperDeliveryLibrary.Models;

/// <summary>
/// This abstract class adds members and functionallity.
/// </summary>
public abstract class ModelBase
{
    /// <summary>
    /// This property contains the ID needed for database access.
    /// <para></para>
    /// This is the primary key in a database table.
    /// </summary>
    public int Id { get; set; } = 0;
}

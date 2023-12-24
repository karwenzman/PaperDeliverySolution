namespace PaperDeliveryLibrary.Models;

/// <summary>
/// This abstract class adds members and functionallity.
/// </summary>
public abstract class ModelBase
{
    /// <summary>
    /// This property contains the data record's primary key.
    /// </summary>
    public int Id { get; set; } = 0;

    /// <summary>
    /// This property contains the the data record's status.
    /// </summary>
    public bool IsActive { get; set; } = true;
}

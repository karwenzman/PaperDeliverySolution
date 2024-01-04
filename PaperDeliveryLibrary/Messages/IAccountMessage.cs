using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryLibrary.Messages;

public interface IAccountMessage
{
    /// <summary>
    /// This property contains the account that shall be displayed by 'AccountViewModel'.
    /// </summary>
    UserModel? Account { get; set; }

    /// <summary>
    /// This property contains the information about the UI's setting. 
    /// Depending on this information the look of the UI is adapted.
    /// </summary>
    SetAccountUserControl SetAccountUserControl { get; set; }
}

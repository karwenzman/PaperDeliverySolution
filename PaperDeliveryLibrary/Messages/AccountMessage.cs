using PaperDeliveryLibrary.Enums;
using PaperDeliveryLibrary.Models;

namespace PaperDeliveryLibrary.Messages;

/// <summary>
/// This class is used to enable the communication between the parent ViewModel and its child ViewModels.
/// <para></para>
/// This class is passing on information about the selected user account. 
/// And information how the UI controls have to be set.
/// </summary>
public class AccountMessage : IAccountMessage
{
    public UserModel? Account { get; set; }

    public SetAccountUserControl SetAccountUserControl { get; set; }
}

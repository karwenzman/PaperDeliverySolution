using PaperDeliveryLibrary.Models;

/// <summary>
/// This class is used to enable the communication between the child ViewModel and its parent ViewModel.
/// <para></para>
/// This class is passing on information that the parent ViewModel must reload its content.
/// </summary>
namespace PaperDeliveryLibrary.Messages;

public class AccountManagerMessage
{
    public bool IsRequestingReload { get; set; } = true;
    public UserModel? UpdatedAccount { get; set; }

}

using PaperDeliveryLibrary.Enums;

namespace PaperDeliveryLibrary.Messages;

/// <summary>
/// This class is used to enable the communication between the parent ViewModel and its child ViewModels.
/// </summary>
public class ShellMessage
{
    public ActivateVisibility SetToActive { get; set; } = ActivateVisibility.None;
    public bool DisplayLoggedOut { get; set; } = false;
    public bool DisplayLoggedIn { get; set; } = false;
    public bool DisplayLogin { get; set; } = false;

}

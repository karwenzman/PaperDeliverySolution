using PaperDeliveryLibrary.Enums;

namespace PaperDeliveryLibrary.Messages;

/// <summary>
/// This class is used to enable the communication between the parent ViewModel and its child ViewModels.
/// </summary>
public class ShellMessage
{
    public LoadViewModel SetToActive { get; set; } = LoadViewModel.ErrorUserControl;
    public bool DisplayLoggedOut { get; set; } = false;
    public bool DisplayLoggedIn { get; set; } = false;
    public bool DisplayLogin { get; set; } = false;

}

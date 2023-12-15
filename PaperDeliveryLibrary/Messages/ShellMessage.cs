using PaperDeliveryLibrary.Enums;

namespace PaperDeliveryLibrary.Messages;

public class ShellMessage
{
    public ActivateVisibility SetToActive { get; set; } = ActivateVisibility.None;
    public bool DisplayLoggedOut { get; set; } = false;
    public bool DisplayLoggedIn { get; set; } = false;
    public bool DisplayLogin { get; set; } = false;

}

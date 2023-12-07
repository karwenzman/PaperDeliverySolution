using System.ComponentModel;
using System.Windows.Input;

namespace PaperDeliveryModernWpf.ViewModels;

public interface IShellViewModel
{
    CommandBinding StopCommand { get; set; }

    void ShellView_Closing(object? sender, CancelEventArgs e);
}

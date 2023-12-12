using System.ComponentModel;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public interface IShellHeaderViewModel
{
    CommandBinding StopCommand { get; set; }
    void ShellView_Closing(object? sender, CancelEventArgs e);

}

using System.ComponentModel;
using System.Windows.Input;

namespace PaperDeliveryWpf.ViewModels;

public interface IShellViewModel
{
    //CommandBinding StopCommand { get; set; }

    string ApplicationHomeDirectory { get; set; }

    string ApplicationName { get; set; }

    //void ShellView_Closing(object? sender, CancelEventArgs e);
}

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellHeaderViewModel : ViewModelBase, IShellHeaderViewModel
{
    // Constructor injection.
    private readonly ILogger<ShellHeaderViewModel> _logger;

    public ShellHeaderViewModel(ILogger<ShellHeaderViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellHeaderViewModel));
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellFooterViewModel : ViewModelBase, IShellFooterViewModel
{
    [ObservableProperty]
    private string _title;

    private readonly ILogger<ShellFooterViewModel> _logger;

    public ShellFooterViewModel(ILogger<ShellFooterViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellFooterViewModel));

        Title = nameof(ShellFooterViewModel);
    }

}

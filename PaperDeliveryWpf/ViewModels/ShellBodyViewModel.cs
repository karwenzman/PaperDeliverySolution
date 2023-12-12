using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class ShellBodyViewModel : ViewModelBase, IShellBodyViewModel
{
    [ObservableProperty]
    private string _title;

    private readonly ILogger<ShellBodyViewModel> _logger;

    public ShellBodyViewModel(ILogger<ShellBodyViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellBodyViewModel));

        Title = nameof(ShellBodyViewModel);
    }
}

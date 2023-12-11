using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public class ShellFooterViewModel : IShellFooterViewModel
{
    private readonly ILogger<ShellFooterViewModel> _logger;

    public ShellFooterViewModel(ILogger<ShellFooterViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellFooterViewModel));

    }
}

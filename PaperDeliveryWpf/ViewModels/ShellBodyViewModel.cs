using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public class ShellBodyViewModel : IShellBodyViewModel
{
    private readonly ILogger<ShellBodyViewModel> _logger;

    public ShellBodyViewModel(ILogger<ShellBodyViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellBodyViewModel));

    }
}

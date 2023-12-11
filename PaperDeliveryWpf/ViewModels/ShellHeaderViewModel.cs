using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public class ShellHeaderViewModel : IShellHeaderViewModel
{
    private readonly ILogger<ShellHeaderViewModel> _logger;

    public ShellHeaderViewModel(ILogger<ShellHeaderViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellHeaderViewModel));

    }
}

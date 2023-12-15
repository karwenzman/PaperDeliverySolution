using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedOutViewModel : ViewModelBase, ILoggedOutViewModel
{
    private readonly ILogger<LoggedOutViewModel> _logger;

    public LoggedOutViewModel(ILogger<LoggedOutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedOutViewModel));
    }
}

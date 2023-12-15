using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedInViewModel : ViewModelBase, ILoggedInViewModel
{
    private readonly ILogger<LoggedInViewModel> _logger;

    public LoggedInViewModel(ILogger<LoggedInViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedInViewModel));
    }
}

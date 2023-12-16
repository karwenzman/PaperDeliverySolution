using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class LoggedOutViewModel : ViewModelBase, ILoggedOutViewModel
{
    private readonly ILogger<LoggedOutViewModel> _logger;

    [ObservableProperty]
    private string _userMessage = string.Empty;

    public LoggedOutViewModel(ILogger<LoggedOutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoggedOutViewModel));

        UserMessage = "You need to login.";
    }
}

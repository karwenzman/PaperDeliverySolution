using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class StartViewModel : ViewModelBase, IStartViewModel
{
    private readonly ILogger<StartViewModel> _logger;

    [ObservableProperty]
    private string _userMessage = string.Empty;

    public StartViewModel(ILogger<StartViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(StartViewModel));

        UserMessage = "You need to login.";
    }
}

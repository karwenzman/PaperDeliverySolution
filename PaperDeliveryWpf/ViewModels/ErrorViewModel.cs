using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class ErrorViewModel : ViewModelBase, IErrorViewModel
{
    private readonly ILogger<ErrorViewModel> _logger;

    public ErrorViewModel(ILogger<ErrorViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ErrorViewModel));
    }
}

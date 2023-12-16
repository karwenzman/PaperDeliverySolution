using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    private readonly ILogger<HomeViewModel> _logger;

    public HomeViewModel(ILogger<HomeViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeViewModel));
    }
}

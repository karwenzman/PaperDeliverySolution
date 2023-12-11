using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public class HomeLoggedOutViewModel : IHomeLoggedOutViewModel
{
    private readonly ILogger<HomeLoggedOutViewModel> _logger;

    public HomeLoggedOutViewModel(ILogger<HomeLoggedOutViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeLoggedOutViewModel));
    }
}

using Microsoft.Extensions.Logging;

namespace PaperDeliveryWpf.ViewModels;

public class HomeLoggedInViewModel : IHomeLoggedInViewModel
{
    private readonly ILogger<HomeLoggedInViewModel> _logger;

    public HomeLoggedInViewModel(ILogger<HomeLoggedInViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(HomeLoggedInViewModel));
    }
}

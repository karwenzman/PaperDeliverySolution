using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperDeliveryWpf.ViewModels;

public class LoginViewModel : ILoginViewModel
{
    private readonly ILogger<LoginViewModel> _logger;

    public LoginViewModel(ILogger<LoginViewModel> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(LoginViewModel));

    }
}

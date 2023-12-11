using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.ViewModels;
using System.Windows;

namespace PaperDeliveryWpf.Views;

public partial class ShellView : Window
{
    // Constructor injection.
    private readonly ILogger<ShellView> _logger;
    private readonly IServiceProvider _serviceProvider;

    // Service provider.
    private readonly IShellViewModel _viewModel;

    public ShellView(ILogger<ShellView> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellView));

        _serviceProvider = serviceProvider;
        _viewModel = serviceProvider.GetRequiredService<IShellViewModel>();

        // Setting up the data binding.
        DataContext = _viewModel;

        CommandBindings.Add(_viewModel.StopCommand);

        // Registering the events.
        Closing += _viewModel.ShellView_Closing;

        InitializeComponent();
    }
}

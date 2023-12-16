using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.ViewModels;
using System.Windows;

namespace PaperDeliveryWpf.Views;

public partial class ShellView : Window
{
    private readonly ILogger<ShellView> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly IShellViewModel _viewModel;

    public ShellView(ILogger<ShellView> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellView));

        _serviceProvider = serviceProvider;
        _viewModel = serviceProvider.GetRequiredService<IShellViewModel>();

        DataContext = _viewModel;

        CommandBindings.Add(_viewModel.StopCommand);

        Closing += _viewModel.ShellView_Closing;

        InitializeComponent();
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.ViewModels;
using System.Windows;

namespace PaperDeliveryWpf.Views;

public partial class ShellView : Window
{
    private readonly IShellViewModel _viewModel;

    private readonly ILogger<ShellView> _logger;

    public ShellView(ILogger<ShellView> logger)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellView));

        _viewModel = App.AppHost!.Services.GetRequiredService<IShellViewModel>();

        DataContext = _viewModel;

        CommandBindings.Add(_viewModel.StopCommand);

        Closing += _viewModel.ShellView_Closing;

        InitializeComponent();
    }
}

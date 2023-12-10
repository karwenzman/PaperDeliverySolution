using Microsoft.Extensions.Logging;
using PaperDeliveryModernWpf.ViewModels;
using System.Windows;

namespace PaperDeliveryModernWpf.Views;

public partial class ShellView : Window
{
    private readonly ILogger<ShellView> _logger;
    private readonly IShellViewModel _viewModel;

    public ShellView(ILogger<ShellView> logger, IShellViewModel viewModel)
    {
        _logger = logger;
        _viewModel = viewModel;
        _logger.LogInformation("* Loading: {class}", nameof(ShellView));

        // Setting up the data binding.
        DataContext = _viewModel;

        CommandBindings.Add(viewModel.StopCommand);

        // Registering the events.
        Closing += viewModel.ShellView_Closing;


        InitializeComponent();
    }
}

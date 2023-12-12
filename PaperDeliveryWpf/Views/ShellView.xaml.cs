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

    // Private fields.
    private readonly IShellViewModel _shellViewModel;
    private readonly IShellHeaderViewModel _shellHeaderViewModel;

    public ShellView(ILogger<ShellView> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellView));

        _serviceProvider = serviceProvider;
        _shellViewModel = serviceProvider.GetRequiredService<IShellViewModel>();
        _shellHeaderViewModel = serviceProvider.GetRequiredService<IShellHeaderViewModel>();

        // Setting up the data binding.
        DataContext = _shellViewModel;

        // Registering the commands.
        CommandBindings.Add(_shellHeaderViewModel.StopCommand);

        // Registering the events.
        Closing += _shellHeaderViewModel.ShellView_Closing;

        InitializeComponent();
    }
}

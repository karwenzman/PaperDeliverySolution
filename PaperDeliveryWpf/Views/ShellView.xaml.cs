using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaperDeliveryWpf.ViewModels;
using System.Windows;

namespace PaperDeliveryWpf.Views;

public partial class ShellView : Window
{
    private readonly ILogger<ShellView> _logger;
    private readonly IServiceProvider _serviceProvider;

    private readonly IShellViewModel _shellViewModel;

    public ShellView(ILogger<ShellView> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _logger.LogInformation("* Loading {class}", nameof(ShellView));

        _serviceProvider = serviceProvider;
        _shellViewModel = serviceProvider.GetRequiredService<IShellViewModel>();

        // Setting up the data binding.
        DataContext = _shellViewModel;

        // Registering the commands.
        CommandBindings.Add(_shellViewModel.StopCommand);

        // Registering the events.
        Closing += _shellViewModel.ShellView_Closing;
        
        InitializeComponent();
    }
}

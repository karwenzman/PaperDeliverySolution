using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.Views;

public partial class ShellBodyView : UserControl
{
    public ShellBodyView()
    {
        var viewModel = (IShellBodyViewModel)App.AppHost!.Services.GetService(typeof(IShellBodyViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(IShellBodyViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(ShellBodyView)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;

        InitializeComponent();
    }
}

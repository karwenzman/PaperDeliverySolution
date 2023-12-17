using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls
{
    /// <summary>
    /// Interaktionslogik für ErrorUserControl.xaml
    /// </summary>
    public partial class ErrorUserControl : UserControl
    {
        public ErrorUserControl()
        {
            var viewModel = (IErrorViewModel)App.AppHost!.Services.GetService(typeof(IErrorViewModel))!;

            if (viewModel == null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    $"The type {nameof(IErrorViewModel)} was not loaded into the dependency injection container!",
                    $"{nameof(ErrorUserControl)}",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.No);
            }

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}

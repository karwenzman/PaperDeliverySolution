using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperDeliveryLibrary.ProjectOptions;

public partial class ApplicationOptions : ObservableObject, IApplicationOptions
{
    [ObservableProperty]
    private string _applicationName = string.Empty;
    [ObservableProperty]
    private string _applicationHomeDirectory = Environment.CurrentDirectory;
}

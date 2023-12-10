using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperDeliveryLibrary.ProjectOptions;

public partial class ApplicationOptions : ObservableObject
{
    [ObservableProperty]
    private string _applicationName;
    [ObservableProperty]
    private string _applicationHomeDirectory;

    public ApplicationOptions()
    {
        _applicationName = string.Empty;
        _applicationHomeDirectory = Environment.CurrentDirectory;
    }
}

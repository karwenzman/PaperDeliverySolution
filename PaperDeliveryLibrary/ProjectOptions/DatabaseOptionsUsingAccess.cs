using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperDeliveryLibrary.ProjectOptions;

public partial class DatabaseOptionsUsingAccess : ObservableObject, IDatabaseOptions
{
    [ObservableProperty]
    private string _databasePath = string.Empty;

    [ObservableProperty]
    private string _databaseTable = string.Empty;

    [ObservableProperty]
    private string _databasePassword = string.Empty;
}

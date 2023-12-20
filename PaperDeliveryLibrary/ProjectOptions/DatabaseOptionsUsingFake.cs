using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperDeliveryLibrary.ProjectOptions;

public partial class DatabaseOptionsUsingFake : ObservableObject, IDatabaseOptions
{
    [ObservableProperty]
    private string _databasePath = string.Empty;
}

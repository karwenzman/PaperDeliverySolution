using CommunityToolkit.Mvvm.ComponentModel;

namespace PaperDeliveryModernWpf.ViewModels;

public partial class HomeViewModel : ViewModelBase, IHomeViewModel
{
    [ObservableProperty]
    private string _userInformation = "Hello User!";
}

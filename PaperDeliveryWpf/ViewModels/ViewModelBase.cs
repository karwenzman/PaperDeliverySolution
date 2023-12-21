using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace PaperDeliveryWpf.ViewModels;

public abstract class ViewModelBase : ObservableValidator, IDisposable
{
    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}

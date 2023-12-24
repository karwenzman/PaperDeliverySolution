using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Security.Claims;
using System.Security.Principal;

namespace PaperDeliveryWpf.ViewModels;

public abstract class ViewModelBase : ObservableValidator, IDisposable
{
    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    internal static IPrincipal GetCurrentApplicationPrincipal()
    {
        ArgumentNullException.ThrowIfNull(Thread.CurrentPrincipal, nameof(Thread.CurrentPrincipal));

        return Thread.CurrentPrincipal;
    }

    internal static IIdentity GetCurrentApplicationIdentity()
    {
        ArgumentNullException.ThrowIfNull(Thread.CurrentPrincipal.Identity, nameof(Thread.CurrentPrincipal.Identity));

        return Thread.CurrentPrincipal.Identity;
    }

    internal static ClaimsPrincipal? GetCurrentOsPrincipal()
    {
        return WindowsPrincipal.Current;
    }

    internal static WindowsIdentity GetCurrentOsIdentity()
    {
        return WindowsIdentity.GetCurrent();
    }
}

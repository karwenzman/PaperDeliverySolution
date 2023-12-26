using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Security.Principal;

namespace PaperDeliveryWpf.ViewModels;

public abstract class ViewModelBase : ObservableValidator, IDisposable
{
    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    internal static void CreateThreadPrincipal(string? userName, string[]? userRoles, string? authenticationType)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        ArgumentNullException.ThrowIfNull(authenticationType, nameof(authenticationType));

        var identity = new GenericIdentity(userName, authenticationType);
        var principal = new GenericPrincipal(identity, userRoles);

        Thread.CurrentPrincipal = principal;
    }

    /// <summary>
    /// This method is setting the <see cref="Thread.CurrentPrincipal"/> to null.
    /// <para></para>
    /// 
    /// </summary>
    internal static void DisposeThreadPrincipal()
    {
        Thread.CurrentPrincipal = null;
    }

    /// <summary>
    /// This method is generating a <see cref="string[]"/> with the user's roles.
    /// <para></para>
    /// The reason for this detour is that the database just stores one role.
    /// But as example the 'admin' has also the role of a 'user'.
    /// <para></para>
    /// Currently these roles are supported:
    /// <br></br>- null => null
    /// <br></br>- guest => guest
    /// <br></br>- user => guest, user
    /// <br></br>- admin => guest, user, admin
    /// </summary>
    /// <param name="userRole">The role stored in the database.</param>
    /// <returns>null, if the user does have no roles, otherwise a list of roles</returns>
    internal static string[]? GetUserRoles(string? userRole)
    {
        if (string.IsNullOrWhiteSpace(userRole))
        {
            return null;
        }
        else if (userRole == "guest")
        {
            return ["guest"];
        }
        else if (userRole == "user")
        {
            return ["guest", "user"];
        }
        else if (userRole == "admin")
        {
            return ["guest", "user", "admin"];
        }
        else
        {
            throw new ArgumentException("Unexpected role stored in database!", nameof(userRole));
        }
    }

    internal static string? GetUserAuthenticationType()
    {
        var principal = Thread.CurrentPrincipal;
        var identity = principal?.Identity;
        return identity?.AuthenticationType;
    }

    internal static string? GetUserName()
    {
        var principal = Thread.CurrentPrincipal;
        var identity = principal?.Identity;
        return identity?.Name;
    }

    internal static bool IsUserInRole(string? userRole)
    {
        if (string.IsNullOrWhiteSpace(userRole))
        {
            throw new ArgumentException("Can not compare the user role with null or empty string!", nameof(userRole));
        }

        var principal = Thread.CurrentPrincipal;
        return principal != null && principal.IsInRole(userRole);
    }

    internal static bool IsUserAuthenticated()
    {
        var principal = Thread.CurrentPrincipal;
        return principal != null && principal.Identity!.IsAuthenticated;
    }
}

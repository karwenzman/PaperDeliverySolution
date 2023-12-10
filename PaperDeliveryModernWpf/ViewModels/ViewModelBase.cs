using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaperDeliveryModernWpf.ViewModels;

public abstract class ViewModelBase : ObservableValidator, IDisposable
{
    public void Dispose()
    {
        // TODO - siehe Nachricht
        throw new NotImplementedException();
    }
}

using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class AccountManagerUserControl : UserControl
{
    public AccountManagerUserControl()
    {
        InitializeComponent();
    }

    private void AccountManagerDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        AccountManagerDataGrid.Focus();
    }
}

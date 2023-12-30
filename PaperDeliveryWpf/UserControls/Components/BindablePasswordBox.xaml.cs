using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls.Components;

public partial class BindablePasswordBox : UserControl
{
    public SecureString Password
    {
        get { return (SecureString)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePasswordBox));


    public BindablePasswordBox()
    {
        InitializeComponent();
        passwordBox.PasswordChanged += BindablePasswordBox_PasswordChanged;
    }

    private void BindablePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = passwordBox.SecurePassword;
    }
}

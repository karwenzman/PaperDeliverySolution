using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls.Components;

public partial class BindablePasswordBox : UserControl
{
    private bool _isPasswordChanging;

    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(string.Empty, PasswordPropertyChanged));


    public BindablePasswordBox()
    {
        InitializeComponent();
    }

    private void BindablePasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _isPasswordChanging = true;
        Password = passwordBox.Password;
        _isPasswordChanging = false;
    }

    private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindablePasswordBox passwordBox)
        {
            passwordBox.UpdatePassword();
        }
    }

    private void UpdatePassword()
    {
        if (!_isPasswordChanging)
        {
            passwordBox.Password = Password;
        }
    }
}

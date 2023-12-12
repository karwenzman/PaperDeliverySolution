﻿using PaperDeliveryWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PaperDeliveryWpf.UserControls;

public partial class Login : UserControl
{
    public Login()
    {
        var viewModel = (ILoginViewModel)App.AppHost!.Services.GetService(typeof(ILoginViewModel))!;

        if (viewModel == null)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(
                $"The type {nameof(ILoginViewModel)} was not loaded into the dependency injection container!",
                $"{nameof(Login)}",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.No);
        }

        DataContext = viewModel;
        InitializeComponent();
    }
}
﻿using System.Windows;
using System.Windows.Controls;



namespace ChicTrash.UI.Page;

public partial class LoginPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;

    public LoginPage(Action<System.Windows.Controls.Page> navigate)
    {
        InitializeComponent();
        _navigate = navigate;

    }

    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        
        _navigate(new RegisterPage(_navigate));
    }

    private void TestClick(object sender, RoutedEventArgs e)
    {
        _navigate(new RegisterPage(_navigate));
    }
}
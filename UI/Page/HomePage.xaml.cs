using ChicTrash.UI.Component;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ChicTrash.UI.Page;

public partial class HomePage : System.Windows.Controls.Page
{
    public readonly DatabaseService _dbService;
    public HomePage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();   
    }

    private void ProfileBalanceCard_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
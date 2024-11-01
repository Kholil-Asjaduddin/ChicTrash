using ChicTrash.UI.Component;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Page;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class HomePage : System.Windows.Controls.Page
{
    public HomePage()
    {
        InitializeComponent();
    }

    private void ProfileBalanceCard_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var homeWindow = Application.Current.Windows.OfType<Home>().FirstOrDefault();
        if (homeWindow != null)
        {
            homeWindow.OpenLoginPage();
        }

    }
}
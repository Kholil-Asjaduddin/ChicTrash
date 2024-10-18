using System.Windows;
using ChicTrash.UI.Page;

namespace ChicTrash.UI.Windows;

public partial class Home : Window
{
    public Home()
    {
        InitializeComponent();
        ContentFrame.Navigate(new HomePage());
    }

   

    private void IconButton_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new HomePage());

    }

    private void IconButton2_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new CommercePage());
    }

    private void IconButton3_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new ArticlePage());
    }
}
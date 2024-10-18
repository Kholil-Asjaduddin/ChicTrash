using System.Windows;
using ChicTrash.UI.Page;

namespace ChicTrash.UI.Windows;

public partial class Home : Window
{
    public Home()
    {
        InitializeComponent();
        ContentFrame.Navigate(new CommercePage());
    }

   

    private void IconButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("This is a test");    
    }
}
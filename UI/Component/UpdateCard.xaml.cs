using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Page;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Component;

public partial class UpdateCard : UserControl
{
    private Item _item;
    public UpdateCard(Item item)
    {
        InitializeComponent();
        _item = item;
    }

    public void NavigateToUpdatePage(object sender, RoutedEventArgs e)
    {
        Application.Current.Windows.OfType<Home>().FirstOrDefault().ContentFrame.Navigate(new UpdateItemPage(_item));
    }

   
}
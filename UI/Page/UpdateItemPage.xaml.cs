using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class UpdateItemPage : System.Windows.Controls.Page
{
    
    DatabaseService dbService;
    private Item _item;
    public UpdateItemPage(Item item)
    {
        InitializeComponent();
        dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
        _item = item;
    }
    
    public void UpdateButton_Click(object sender, RoutedEventArgs e)
    {
       dbService.EditItem(_item.ItemId, DescriptionTxtBox.Text, PriceTxtBox.Text, QuantityTxtBox.Text, ImageTxtBox.Text);
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        dbService.DeleteItem(_item.ItemId);
    }
}
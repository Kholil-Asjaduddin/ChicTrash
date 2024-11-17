using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class SellerItemPage : System.Windows.Controls.Page
{
    
    public Seller _user;
    public DatabaseService _dbService;
    public List<Item> _items;
    public SellerItemPage()
    {
        InitializeComponent();
        _dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
        _user  = Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentSeller;
    }
    
    private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
              
        }
    }

    private void ItemPanel_OnLoaded(object sender, RoutedEventArgs e)
    {
        var itemOrdered = _dbService.getSellerItem(_user.SellerId);
        foreach (var item in itemOrdered)
        {
            var card = new UpdateCard(item)
            {
                Margin = new Thickness(0, 30, 1350, 0)
            };
            card.NameTxtBlock.Text = item.ItemName;
            card.PriceTxtBlock.Text = item.Price.ToString();
            card.QuantityTxtBlock.Text = item.Quantity.ToString();
            ItemPanel.Children.Add(card);
        }
            

    }
    
    
    
}
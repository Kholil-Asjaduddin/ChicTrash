using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class SellerOrderPage : System.Windows.Controls.Page
{
    public User _user;
    public DatabaseService _dbService;
    public double totalPrice;

    public SellerOrderPage()
    {
        InitializeComponent();
        _dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
        _user  = Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentUser;

    }

    private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
              
        }
    }

    private void ItemPanel_OnLoaded(object sender, RoutedEventArgs e)
    {
        var itemCart = _dbService.getUserCart(_user.UserUserId);
        foreach (var item in itemCart)
        {
            var card = new CartItemCard
            {
                Margin = new Thickness(0, 30, 1350, 0)
            };
            card.NameTxtBlock.Text = item.itemName;
            card.PriceTxtBlock.Text = item.itemPrice.ToString();
            card.QuantityTxtBlock.Text = item.itemQuantity.ToString();
            totalPrice += item.itemPrice * item.itemQuantity;
            ItemPanel.Children.Add(card);
        }
            
        TotalPriceTxtBlock.Text = "RP" + totalPrice.ToString();

    }

    private void CheckoutButton_OnClick(object sender, RoutedEventArgs e)
    {
        _dbService.checkoutItems(totalPrice, _user.UserUserId);
        MessageBox.Show("Item Successfully Checked");
    }
}

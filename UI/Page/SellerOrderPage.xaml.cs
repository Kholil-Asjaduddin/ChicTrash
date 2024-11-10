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
        var itemOrdered = _dbService.GetOrders(_user.UserUserId);
        foreach (var item in itemOrdered)
        {
            var card = new CartItemCard
            {
                Margin = new Thickness(0, 30, 1350, 0)
            };
            card.NameTxtBlock.Text = item.ProductName;
            card.PriceTxtBlock.Text = item.Address;
            card.QuantityTxtBlock.Text = item.Quantity.ToString();
            ItemPanel.Children.Add(card);
        }
            

    }

    private void CheckoutButton_OnClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Item Successfully Checked");
    }
}

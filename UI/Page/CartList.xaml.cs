using System.Windows;
using System.Windows.Input;
using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page
{

    public partial class CartList : System.Windows.Controls.Page
    {
        public User _user;
        public DatabaseService _dbService;
        public double totalPrice;
        public List<int> cartItemIds;

        public CartList()
        {
            InitializeComponent();
            _dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
            _user = Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentUser;
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Handle mouse down event if needed
            }
        }

        private void ItemPanel_OnLoaded(object sender, RoutedEventArgs e)
        {
            var cartItems = _dbService.getUserCart(_user.UserUserId);
            cartItemIds = cartItems.Select(cart => cart.itemId).ToList();

            foreach (var item in cartItems)
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
            _dbService.CheckoutItems(cartItemIds, totalPrice); // Pass only cartItemIds and totalPrice to checkoutItems
            MessageBox.Show("Item Successfully Checked");
        }
    }
}

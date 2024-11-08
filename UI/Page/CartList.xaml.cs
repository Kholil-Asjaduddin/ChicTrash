using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page
{
    
    public partial class CartList : System.Windows.Controls.Page
    {
        public User _user;
        public DatabaseService _dbService;

        public CartList()
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
            MessageBox.Show(itemCart.Count().ToString());
            foreach (var item in itemCart)
            {
                var card = new CartItemCard
                {
                    Margin = new Thickness(0, 30, 1350, 0)
                };
                card.NameTxtBlock.Text = item.itemName;
                card.DescriptionTxtBlock.Text = item.itemDescription;
                card.QuantityTxtBlock.Text = item.itemQuantity.ToString();
                ItemPanel.Children.Add(card);
            }

        }
    }
}

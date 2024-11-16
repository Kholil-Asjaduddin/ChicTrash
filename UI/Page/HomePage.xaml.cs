using ChicTrash.UI.Component;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Page;
using ChicTrash.UI.Windows;
using System.Windows.Media.Imaging;
using Sprache;

namespace ChicTrash.UI.Page;

public partial class HomePage : System.Windows.Controls.Page
{
    private Customer _customer;
    private Seller _seller;
    private User _user;
    private Home homeWindow;
    

    
    public HomePage()
    {
        InitializeComponent();

        homeWindow = Application.Current.Windows.OfType<Home>().FirstOrDefault();
        FetchUserInfo();
        UpdateProfileBalanceCard();
        LoadItems();
    }

    private void FetchUserInfo()
    {
        if (homeWindow != null)
        {
            _customer = homeWindow.CurrentCustomer;
            _seller = homeWindow.CurrentSeller;
            _user = homeWindow.CurrentUser;
        }
    }

    private void UpdateProfileBalanceCard()
    {
        if (_customer != null)
        {
            profileBalanceCard.userName.Text = _customer.UserName;
            profileBalanceCard.userBalance.Text = "Rp " + _customer.UserMoney.ToString("N0");
        }
        else if (_seller != null)
        {
            profileBalanceCard.userName.Text = _seller.UserName;
            profileBalanceCard.userBalance.Text = "Rp " + _seller.UserMoney.ToString("N0");
        }
        else
        {
            profileBalanceCard.userName.Text = "Welcome User";
            profileBalanceCard.userBalance.Text = "Rp -";
        }
    }
    private void ProfileBalanceCard_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (homeWindow != null)
        {
            if (_customer == null && _seller == null)
            {
                homeWindow.OpenLoginPage();
            }
            else
            {
                homeWindow.ContentFrame.Navigate(new UpdateProfilePage());
            }
        }

    }

    private void profileBalanceCard_Loaded(object sender, RoutedEventArgs e)
    {
        FetchUserInfo();
        UpdateProfileBalanceCard();
    }

    private void LoadItems()
    {
        if (homeWindow != null)
        {
            var items = homeWindow._dbService.GetItems();

            // Batasi jumlah item yang ditampilkan menjadi 4
            int maxItemsToShow = 4;
            int itemsCount = Math.Min(items.Count, maxItemsToShow);

            for (int i = 0; i < itemsCount; i++)
            {
                var item = items[i];
                var itemCard = new ItemCard();
                itemCard.productName.Text = item.ItemName;
                itemCard.productPrice.Text = "Rp " + item.Price.ToString("N0");

                if (!string.IsNullOrEmpty(item.Image))
                {
                    itemCard.productImage.Source = new BitmapImage(new Uri(item.Image, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    itemCard.productImage.Source = new BitmapImage(new Uri("path/to/default/image.png", UriKind.RelativeOrAbsolute));
                }

                // Add MouseLeftButtonUp event handler
                itemCard.MouseLeftButtonUp += (sender, e) =>
                {
                    homeWindow.ContentFrame.Navigate(new ItemPage(item ));
                };

                // Set margin dynamically
                itemCard.Margin = new Thickness(10);
                wrapPanel.Children.Add(itemCard);
            }
        }
    }
}
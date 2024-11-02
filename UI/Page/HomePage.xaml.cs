using ChicTrash.UI.Component;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Page;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class HomePage : System.Windows.Controls.Page
{
    private Customer _customer;
    private Seller _seller;
    private Home homeWindow;
    public HomePage()
    {
        InitializeComponent();
        homeWindow = Application.Current.Windows.OfType<Home>().FirstOrDefault();
    }

    private void FetchUserInfo()
    {
        if (homeWindow != null)
        {
            _customer = homeWindow.CurrentCustomer;
            _seller = homeWindow.CurrentSeller;
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
            homeWindow.OpenLoginPage();
        }

    }

    private void profileBalanceCard_Loaded(object sender, RoutedEventArgs e)
    {
        FetchUserInfo();
        UpdateProfileBalanceCard();
    }
}
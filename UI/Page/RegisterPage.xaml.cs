using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;
using Npgsql;

namespace ChicTrash.UI.Page;

public partial class RegisterPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;
    
    
    private static DatabaseService _dbService;
    
    public RegisterPage(Action<System.Windows.Controls.Page> navigate, DatabaseService databaseService)
    {
        InitializeComponent();
        _dbService = databaseService;
        _navigate = navigate;

    }

    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        User newAccount = new User();
        newAccount.UserName = UsernameTxtBox.Text;
        newAccount.UserEmail = EmailTxtBox.Text;
        newAccount.UserPassword = PasswordTxtBox.Text;
        newAccount.UserPhone = PhoneTxtBox.Text;
        newAccount.UserAdress = AddressTxtBox.Text;
        newAccount.UserMoney = 0;
        newAccount.SellerId = null;
        newAccount.CustomerId = null;
        if (_dbService.registerUser(newAccount, RadioButton_Buyer.IsChecked.Value, RadioButton_Seller.IsChecked.Value))
        {
            Application.Current.Windows.OfType<Home>().FirstOrDefault()?.SetUserRole(_dbService.GetUserById(_dbService.GetUserIdByEmail(EmailTxtBox.Text)).UserUserId);
            Application.Current.Windows.OfType<Home>().FirstOrDefault()?.ContentFrame.Navigate(new HomePage());
        }
         
        
    }

    private void TestClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Test Click");
    }
}
using System.Windows;
using ChicTrash.UI.Page;

namespace ChicTrash.UI.Windows;

public partial class Home : Window
{
    public readonly DatabaseService _dbService;
    public Customer CurrentCustomer { get; private set; }
    public Seller CurrentSeller { get; private set; }
    public Home()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        ContentFrame.Navigate(new ItemPage());
    }

   

    private void IconButton_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new HomePage());

    }

    private void IconButton2_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new CommercePage());
    }

    private void IconButton3_OnClick(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(new ArticlePage());
    }

    private void IconButton_Loaded(object sender, RoutedEventArgs e)
    {

    }

    public void OpenLoginPage()
    {
        LoginPage loginPage = new LoginPage(_dbService, page => ContentFrame.Navigate(page));
        ContentFrame.Navigate(loginPage);
    }

    public void SetUserRole(int userId)
    {
        var user = _dbService.GetUserById(userId);

        if (user.SellerId == null)
        {
            CurrentCustomer = new Customer
            {
                UserUserId = user.UserUserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserPhone = user.UserPhone,
                UserAdress = user.UserAdress,
                UserMoney = user.UserMoney,
                CustomerId = user.CustomerId.ToString()
            };
        }
        else
        {
            CurrentSeller = new Seller
            {
                UserUserId = user.UserUserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserPhone = user.UserPhone,
                UserAdress = user.UserAdress,
                UserMoney = user.UserMoney,
                SellerId = user.SellerId.ToString()
            };
        }
    }
}
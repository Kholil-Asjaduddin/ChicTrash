using System.Windows;
using ChicTrash.UI.Page;

namespace ChicTrash.UI.Windows;

public partial class Home : Window
{
    public readonly DatabaseService _dbService;
    public Customer CurrentCustomer { get; private set; }
    public Seller CurrentSeller { get; private set; }
    public User CurrentUser { get; private set; }
    public readonly Action<System.Windows.Controls.Page> _navigate;        
    public Home()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
        OpenLoginPage();        

        // Set the window to fit the screen size and allow resizing
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.Width = SystemParameters.WorkArea.Width;
        this.Height = SystemParameters.WorkArea.Height;
        this.ResizeMode = ResizeMode.CanResize;
    }

    public void setUser(User user)
    {
        CurrentUser = user;
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
        LoginPage loginPage = new LoginPage(_dbService, _navigate);
        ContentFrame.Navigate(loginPage);
    }

    public void SetUserRole(int userId)
    {
        
        try
        {
            var user = _dbService.GetUserById(userId);
            CurrentUser = new User
            {
                
                UserUserId = user.UserUserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserPhone = user.UserPhone,
                UserAdress = user.UserAdress,
                UserMoney = user.UserMoney,
                
            };
            MessageBox.Show(CurrentUser.ToString());
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        

        // if (user.SellerId == null)
        // {
            
        // }
        // else
        // {
        //     CurrentSeller = new Seller
        //     {
        //         UserUserId = user.UserUserId,
        //         UserName = user.UserName,
        //         UserEmail = user.UserEmail,
        //         UserPassword = user.UserPassword,
        //         UserPhone = user.UserPhone,
        //         UserAdress = user.UserAdress,
        //         UserMoney = user.UserMoney,
        //         SellerId = user.SellerId.ToString()
        //     };
        // }
    }
}
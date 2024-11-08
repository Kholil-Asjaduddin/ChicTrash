using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;
using Npgsql; 



namespace ChicTrash.UI.Page;

public partial class LoginPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;
    private readonly DatabaseService _dbService;
    public LoginPage(DatabaseService dbService, Action<System.Windows.Controls.Page> navigate)
    {
        InitializeComponent();
        _dbService = dbService;
        _navigate = navigate;
    }

    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_dbService.ValidateUser(EmailTxtBox.Text, PasswordTxtBox.Text))
        {
            var homeWindow = Application.Current.Windows.OfType<Home>().FirstOrDefault();
            if (homeWindow != null)
            {
                homeWindow.SetUserRole(_dbService.GetUserIdByEmail(EmailTxtBox.Text));
                _navigate(new HomePage());
            }
        }
        else
        {
            MessageBox.Show("Email or password is incorrect");
        }
    }

    private void TestClick(object sender, RoutedEventArgs e)
    {
        _navigate(new RegisterPage(_navigate, _dbService));
    }
}
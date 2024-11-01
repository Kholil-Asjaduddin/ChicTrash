using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;
using Npgsql; 



namespace ChicTrash.UI.Page;

public partial class LoginPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;
    private readonly DatabaseService _dbService;

    public LoginPage(Action<System.Windows.Controls.Page> navigate)
    {
        InitializeComponent();
        _navigate = navigate;
        _dbService = new DatabaseService();
    }

    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_dbService.ValidateUser(EmailTxtBox.Text, PasswordTxtBox.Text))
        {
            Home home = new();
            home.Show();
        }
        else
        {
            MessageBox.Show("Email or password is incorrect");

        }
    }
    private void TestClick(object sender, RoutedEventArgs e)
    {
        _navigate(new RegisterPage(_navigate));
    }
}
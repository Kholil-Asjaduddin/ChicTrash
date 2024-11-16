using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class UpdateProfilePage : System.Windows.Controls.Page
{
    DatabaseService _dbService;
    
    public UpdateProfilePage()
    {
        _dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
        InitializeComponent();
    }

    public void TopUpBalance()
    {

    }

    public void UpdateProfile(object sender, RoutedEventArgs e)
    {
        _dbService.UpdateUserDetails(Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentUser.UserUserId, AddressTxtBox.Text, PhoneNumberTxtBox.Text, NewPasswordTxtBox.Text);
    }
}
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;
using Npgsql; 



namespace ChicTrash.UI.Page;

public partial class LoginPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;
    public static string connstring = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    public NpgsqlConnection conn = new NpgsqlConnection(connstring);

    public LoginPage(Action<System.Windows.Controls.Page> navigate)
    {
        InitializeComponent();
        _navigate = navigate;

    }

    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        conn.Open();
        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM user_table WHERE email = @email AND password = @password", conn);
        try
        {
            cmd.Parameters.AddWithValue("email", EmailTxtBox.Text);
            cmd.Parameters.AddWithValue("password", PasswordTxtBox.Text);
            if (cmd.ExecuteScalar() != null)
            {
                Home Home = new Home();
                Home.Show();
                conn.Close();
            }
            else
            {
                MessageBox.Show("Email or password is incorrect");
            }

            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            conn.Close();
        }
    }

    private void TestClick(object sender, RoutedEventArgs e)
    {
        _navigate(new RegisterPage(_navigate));
    }
}
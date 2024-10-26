using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;
using Npgsql;

namespace ChicTrash.UI.Page;

public partial class RegisterPage : System.Windows.Controls.Page
{
    private readonly Action<System.Windows.Controls.Page> _navigate;
    public static string connstring = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
    public NpgsqlConnection conn = new NpgsqlConnection(connstring);
    
    public RegisterPage(Action<System.Windows.Controls.Page> navigate)
    {
        InitializeComponent();
        _navigate = navigate;

    }
    private void RoundedButton_OnClick(object sender, RoutedEventArgs e)
    {
        conn.Open();
        try
        {
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO user_table (user_name, email, password, phone, address, money) VALUES (@user_name , @email, @password, '','',0)", conn);
            command.Parameters.AddWithValue("user_name", UsernameTxtBox.Text);
            command.Parameters.AddWithValue("email", EmailTxtBox.Text);
            command.Parameters.AddWithValue("password", PasswordTxtBox.Text);
            command.ExecuteNonQuery();
            MessageBox.Show("User created successfully");
            conn.Close();
            Home page = new Home();
            page.Show();
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            conn.Close();
        }
    }

    private void TestClick(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Test Click");
    }
}
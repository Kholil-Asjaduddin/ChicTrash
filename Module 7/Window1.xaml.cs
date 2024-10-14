using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChicTrash.Module_7;

namespace ChicTrash.Module_7
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee();
            employee.LoginName = tbUsername.Text;
            employee.Password = tbPassword.Password;
            if (employee.Login(employee.LoginName, employee.Password))
                MessageBox.Show("Login berhasil, ID anda adalah " + employee.EmployeeID.ToString());
            else
                        MessageBox.Show("Login Gagal");
        }
    }
}

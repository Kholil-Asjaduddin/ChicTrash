using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChicTrash.UI.Page;
using ChicTrash.UI.Windows;

namespace ChicTrash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(new LoginPage(NavigateToPage));
        }
        
        private void NavigateToPage(Page? page)
        {
            ContentFrame.Navigate(page);
        }
    }
}
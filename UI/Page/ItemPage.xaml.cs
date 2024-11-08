using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class ItemPage : System.Windows.Controls.Page
{
    private Item _item;
    private readonly Action<System.Windows.Controls.Page> _navigate;
    public ItemPage(Item item)
    {
        InitializeComponent();
        _item = item;
        LoadItemDetails();
    }

    private void RoundedButton_OnClick(object sender, System.Windows.RoutedEventArgs e)
    {
        Application.Current.Windows.OfType<Home>().FirstOrDefault()?.ContentFrame.Navigate(new CartList());
    }

    private void RoundedButton_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void RoundedButton_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
    {

    }

    private void LoadItemDetails()
    {
        productName.Text = _item.ItemName;
        priceTag.Content = "Rp " + _item.Price.ToString("N0");
        productInformation.Text = _item.Description;

        if (!string.IsNullOrEmpty(_item.Image))
        {
            productImage.Source = new BitmapImage(new Uri(_item.Image, UriKind.RelativeOrAbsolute));
        }
        else
        {
            productImage.Source = new BitmapImage(new Uri("path/to/default/image.png", UriKind.RelativeOrAbsolute));
        }
    }
}
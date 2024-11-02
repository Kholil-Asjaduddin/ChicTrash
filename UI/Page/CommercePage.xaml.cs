using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChicTrash.UI.Page;

public partial class CommercePage : System.Windows.Controls.Page
{
    private Home homeWindow;

    public CommercePage()
    {
        InitializeComponent();
        homeWindow = Application.Current.Windows.OfType<Home>().FirstOrDefault();
        LoadItemsIntoWrapPanel(wrapPanelWaste, "Waste");
        LoadItemsIntoWrapPanel(wrapPanelRecycled, "Recycled");
    }

    private void LoadItemsIntoWrapPanel(WrapPanel wrapPanel, string category)
    {
        if (homeWindow != null)
        {
            var items = homeWindow._dbService.GetItems().Where(item => item.Category == category).ToList();
            foreach (var item in items)
            {
                var itemCard = new ItemCard
                {
                    Margin = new Thickness(10)
                };
                itemCard.productName.Text = item.ItemName;
                itemCard.productPrice.Text = "Rp " + item.Price.ToString("N0");

                try
                {
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        Uri uriResult;
                        bool isValidUri = Uri.TryCreate(item.Image, UriKind.RelativeOrAbsolute, out uriResult)
                                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                        if (isValidUri)
                        {
                            itemCard.productImage.Source = new BitmapImage(uriResult);
                        }
                        else
                        {
                            throw new Exception("Invalid URI format");
                        }
                    }
                    else
                    {
                        itemCard.productImage.Source = new BitmapImage(new Uri("path/to/default/image.png", UriKind.RelativeOrAbsolute));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image for item {item.ItemName}: {ex.Message}");
                    itemCard.productImage.Source = new BitmapImage(new Uri("path/to/default/image.png", UriKind.RelativeOrAbsolute));
                }

                wrapPanel.Children.Add(itemCard);
            }
        }
    }
}
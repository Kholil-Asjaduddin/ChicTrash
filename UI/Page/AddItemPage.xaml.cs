using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class AddItemPage : System.Windows.Controls.Page
{
    DatabaseService dbService;
    public AddItemPage()
    {
        InitializeComponent();
        dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()._dbService;
    }

    public void UploadButton_Click(object sender, RoutedEventArgs e)
    {
        if (RecycledRadioButton.IsChecked == true)
        {
            dbService.UploadItems(Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentSeller.SellerId,
                NameTxtBox.Text, int.Parse(QuantityTxtBox.Text), double.Parse(PriceTxtBox.Text), DescriptionTxtBox.Text, ImageTxtBox.Text, "Recycled");
        }
        else if (WasteRadioButton.IsChecked == true)
        {
            dbService.UploadItems(Application.Current.Windows.OfType<Home>().FirstOrDefault().CurrentSeller.SellerId,
                NameTxtBox.Text, int.Parse(QuantityTxtBox.Text), double.Parse(PriceTxtBox.Text), DescriptionTxtBox.Text, ImageTxtBox.Text, "Waste");
        }
        else
        {
            MessageBox.Show("Pick A Category");
        }
    }

   
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChicTrash.UI.Component;

public partial class ItemCard : UserControl
{
    public ItemCard()
    {
        InitializeComponent();
    }
    public static readonly DependencyProperty IconSourceProperty =
        DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(ItemCard), new PropertyMetadata(null));

    public ImageSource IconSource
    {
        get { return (ImageSource)GetValue(IconSourceProperty); }
        set { SetValue(IconSourceProperty, value); }
    }

    // Dependency property for the top text
    public static readonly DependencyProperty TopTextProperty =
        DependencyProperty.Register("TopText", typeof(string), typeof(ItemCard), new PropertyMetadata("Lorem"));

    public string TopText
    {
        get { return (string)GetValue(TopTextProperty); }
        set { SetValue(TopTextProperty, value); }
    }

    // Dependency property for the bottom text
    public static readonly DependencyProperty BottomTextProperty =
        DependencyProperty.Register("BottomText", typeof(string), typeof(ItemCard), new PropertyMetadata("Ipsum"));

    public string BottomText
    {
        get { return (string)GetValue(BottomTextProperty); }
        set { SetValue(BottomTextProperty, value); }
    }
    
    public event RoutedEventHandler Click;

    // Click event handler
    private void OnClick(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e); // Raise the Click event to allow parent to handle it
    }
}
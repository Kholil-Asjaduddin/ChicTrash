using System.Windows;
using System.Windows.Controls;

namespace ChicTrash.UI.Component;

public partial class RoundedTextBox : UserControl
{
    public RoundedTextBox()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(RoundedTextBox), 
            new FrameworkPropertyMetadata(string.Empty));

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
}
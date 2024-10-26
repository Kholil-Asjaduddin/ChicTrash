using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChicTrash.UI.Component
{
    public partial class HyperlinkButton : UserControl
    {
        public HyperlinkButton()
        {
            InitializeComponent();
        }

        // Dependency property for the Text (hyperlink content)
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HyperlinkButton),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Dependency property for the Foreground color
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Brush), typeof(HyperlinkButton),
                new PropertyMetadata(Brushes.Blue));

        public Brush ForegroundColor
        {
            get { return (Brush)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }
        
        // Event for handling the click event
        public event RoutedEventHandler Click;

        // Click event handler
        private void OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e); // Raise the Click event to allow parent to handle it
        }
    }
}
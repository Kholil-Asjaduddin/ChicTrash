using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChicTrash.UI.Component
{
    public partial class IconButton : UserControl
    {
        public IconButton()
        {
            InitializeComponent();
        }

        // Dependency property for the Icon Source
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(IconButton), new PropertyMetadata(null));

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Dependency property for the Button Background
        public static readonly DependencyProperty ButtonBackgroundProperty =
            DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(IconButton), new PropertyMetadata(Brushes.LightGray));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }

        // Dependency property for the Hover Background
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(IconButton), new PropertyMetadata(Brushes.Gray));

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
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
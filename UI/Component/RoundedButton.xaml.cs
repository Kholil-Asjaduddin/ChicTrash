using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChicTrash.UI.Component
{
    public partial class RoundedButton : UserControl
    {
        public RoundedButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(RoundedButton), new PropertyMetadata("Rounded Button"));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly DependencyProperty ButtonBackgroundProperty =
            DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(RoundedButton), new PropertyMetadata(Brushes.Gray));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty ButtonBackgroundHoverProperty = 
            DependencyProperty.Register("ButtonBackgroundHover", typeof(Brush), typeof(RoundedButton), new PropertyMetadata(Brushes.Gray));

        public Brush ButtonBackgroundHover
        {
            get { return (Brush)GetValue(ButtonBackgroundHoverProperty); }
            set { SetValue(ButtonBackgroundHoverProperty, value); }
        }
        
        public event RoutedEventHandler Click;

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
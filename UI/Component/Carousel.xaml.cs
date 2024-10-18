using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChicTrash.UI.Component;

public partial class Carousel : UserControl
{
    private int _currentIndex = 0;
        
        // Observable collection to hold the carousel items
        public ObservableCollection<string> Items { get; set; }

        public Carousel()
        {
            InitializeComponent();
            Items = new ObservableCollection<string>
            {
                "Item 1", "Item 2", "Item 3", "Item 4", "Item 5"
            };

            // Set data context to the UserControl to bind data
            DataContext = this;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                ScrollToCurrentItem();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < Items.Count - 1)
            {
                _currentIndex++;
                ScrollToCurrentItem();
            }
        }

        private void ScrollToCurrentItem()
        {
            // Access the ScrollViewer by walking up the visual tree
            var scrollViewer = FindScrollViewer(CarouselItemsControl);

            if (scrollViewer != null)
            {
                // Calculate the scroll offset based on the current index and item width
                double itemWidth = 100; // Set this to the width of your items
                double offset = _currentIndex * itemWidth;

                // Scroll to the calculated horizontal offset
                scrollViewer.ScrollToHorizontalOffset(offset);
            }
        }

        // Helper method to find the ScrollViewer in the ItemsControl
        private ScrollViewer FindScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
            {
                return obj as ScrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = FindScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
}
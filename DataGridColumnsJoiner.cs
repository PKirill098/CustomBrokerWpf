using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class DataGridColumnsJoiner : UIElement
    {
        private static bool noChange;
        public static readonly DependencyProperty FirstDisplayIndexProperty;
        public static readonly DependencyProperty SecondDisplayIndexProperty;

        static DataGridColumnsJoiner()
        {
            noChange = true;
            FrameworkPropertyMetadata metadataFirst = new FrameworkPropertyMetadata(0, new PropertyChangedCallback(onFirstDisplayIndexPropertyChanged));
            FrameworkPropertyMetadata metadataSecond = new FrameworkPropertyMetadata(0, new PropertyChangedCallback(onSecondDisplayIndexPropertyChanged));
            FirstDisplayIndexProperty = DependencyProperty.RegisterAttached("FirstDisplayIndex", typeof(object), typeof(DataGridColumnsJoiner), metadataFirst);
            SecondDisplayIndexProperty = DependencyProperty.RegisterAttached("SecondDisplayIndex", typeof(object), typeof(DataGridColumnsJoiner), metadataSecond);
        }

        public static object GetFirstDisplayIndex(UIElement element)
        {
            return element.GetValue(DataGridColumnsJoiner.FirstDisplayIndexProperty);
        }
        public static void SetFirstDisplayIndex(UIElement element, object value)
        {
            element.SetValue(DataGridColumnsJoiner.FirstDisplayIndexProperty, value);
        }
        public static object GetSecondDisplayIndex(UIElement element)
        {
            return element.GetValue(DataGridColumnsJoiner.SecondDisplayIndexProperty);
        }
        public static void SetSecondDisplayIndex(UIElement element, object value)
        {
            element.SetValue(DataGridColumnsJoiner.SecondDisplayIndexProperty, value);
        }

        public static void onFirstDisplayIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            int newValue=(int)e.NewValue;
            int SecondIndex = (int)(sender as UIElement).GetValue(DataGridColumnsJoiner.SecondDisplayIndexProperty);
            if (newValue < 0 | SecondIndex < 0) return;
            if (newValue < SecondIndex | !noChange)
            {
                if (newValue > SecondIndex)
                {
                    noChange = true;
                    (sender as UIElement).SetValue(DataGridColumnsJoiner.SecondDisplayIndexProperty, newValue);
                }
                else if (newValue + 1 != SecondIndex)
                    (sender as UIElement).SetValue(DataGridColumnsJoiner.SecondDisplayIndexProperty, newValue + 1);
                noChange = true;
            }
            else
                noChange = false;
        }
        public static void onSecondDisplayIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            int newValue = (int)e.NewValue;
            int FistIndex = (int)(sender as UIElement).GetValue(DataGridColumnsJoiner.FirstDisplayIndexProperty);
            if (newValue < 0 | FistIndex < 0) return;
            if (FistIndex < newValue | !noChange)
            {
                if (FistIndex > newValue)
                {
                    noChange = true;
                    (sender as UIElement).SetValue(DataGridColumnsJoiner.FirstDisplayIndexProperty, newValue);
                }
                else if (FistIndex != newValue - 1)
                    (sender as UIElement).SetValue(DataGridColumnsJoiner.FirstDisplayIndexProperty, newValue - 1);
                noChange = true;
            }
            else
                noChange = false;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Tathkīr_WPF.Views.GenericViews
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        public HomeControl()
        {
            InitializeComponent();
            DataContext = new ViewModels.Generic.HomeViewModel(this);
        }
        private void HijriCalendar_Loaded(object sender, RoutedEventArgs e)
        {
            HighlightEventDates();
        }

        private void HijriCalendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            HighlightEventDates();
        }

        private void HijriCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            HighlightEventDates();
        }

        public void HighlightEventDates()
        {
            if (DataContext is not ViewModels.Generic.HomeViewModel vm)
                return;

            var currentMonth = HijriCalendar.DisplayDate.Month;
            var currentYear = HijriCalendar.DisplayDate.Year;

            foreach (var button in FindVisualChildren<CalendarDayButton>(HijriCalendar))
            {
                if (button.DataContext is DateTime date)
                {
                    // Skip days not in the current displayed month
                    if (date.Month != currentMonth || date.Year != currentYear)
                    {
                        button.ClearValue(BackgroundProperty);
                        button.ClearValue(ForegroundProperty);
                        button.ClearValue(TemplateProperty);
                        button.ClearValue(ToolTipProperty);
                        continue;
                    }

                    // Check if event exists for this date
                    var eventForDate = vm.HijriEvents.FirstOrDefault(ev => ev.Date.Date == date.Date);
                    if (eventForDate != null)
                    {
                        button.Background = new SolidColorBrush(Colors.OrangeRed);
                        button.Foreground = Brushes.White;

                        // Add tooltip with event title
                        button.ToolTip = eventForDate.Title;

                        // Ensure square dimensions
                        button.Width = 30;
                        button.Height = 30;
                        button.HorizontalContentAlignment = HorizontalAlignment.Center;
                        button.VerticalContentAlignment = VerticalAlignment.Center;
                        button.Padding = new Thickness(0);

                        // Set initial tooltip delay to 0 for immediate display
                        ToolTipService.SetInitialShowDelay(button, 0);

                        // Create the template
                        var borderFactory = new FrameworkElementFactory(typeof(Border));
                        borderFactory.SetValue(Border.BackgroundProperty, button.Background);
                        borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(15));
                        borderFactory.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                        borderFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Stretch);

                        // Add ContentPresenter inside the border
                        var contentFactory = new FrameworkElementFactory(typeof(ContentPresenter));
                        contentFactory.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
                        contentFactory.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);

                        borderFactory.AppendChild(contentFactory);

                        // Assign the template
                        var template = new ControlTemplate(typeof(CalendarDayButton))
                        {
                            VisualTree = borderFactory
                        };

                        button.Template = template;
                    }
                    else
                    {
                        // Reset default style
                        button.ClearValue(BackgroundProperty);
                        button.ClearValue(ForegroundProperty);
                        button.ClearValue(TemplateProperty);
                        button.ClearValue(ToolTipProperty);
                    }
                }
            }
        }

        // Helper to get visual children
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t)
                {
                    yield return t;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

    }
}

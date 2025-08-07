using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Tathkīr_WPF.Converters
{
    public class ConditionalBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return Brushes.Transparent;

            if (values[0] is bool isNext && values[1] is Brush brush)
            {
                return isNext ? brush : Brushes.Transparent;
            }

            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}

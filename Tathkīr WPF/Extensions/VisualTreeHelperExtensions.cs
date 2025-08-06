using System.Windows.Media;
using System.Windows;

namespace Tathkīr_WPF.Extensions
{
    public static class VisualTreeHelperExtensions
    {
        public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject? current = child;

            while (current != null)
            {
                if (current is T target)
                    return target;

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
    }

}

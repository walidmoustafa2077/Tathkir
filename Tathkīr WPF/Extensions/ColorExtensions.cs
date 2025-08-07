using System.Windows.Media;

namespace Tathkīr_WPF.Extensions
{
    public static class ColorExtensions
    {
        public static Color Darken(this Color color, double factor)
        {
            if (factor < 0.0 || factor > 1.0)
                throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0.0 and 1.0.");

            return Color.FromRgb(
                (byte)(color.R * factor),
                (byte)(color.G * factor),
                (byte)(color.B * factor));
        }

        public static Color Lighten(this Color color, double factor)
        {
            if (factor < 0.0 || factor > 1.0)
                throw new ArgumentOutOfRangeException(nameof(factor), "Amount must be between 0.0 and 1.0");

            byte Lerp(byte from, byte to) => (byte)(from + (to - from) * factor);

            return Color.FromRgb(
                Lerp(color.R, 255),
                Lerp(color.G, 255),
                Lerp(color.B, 255));
        }

    }
}

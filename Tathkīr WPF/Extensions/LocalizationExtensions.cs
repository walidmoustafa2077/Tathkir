using System.Reflection;

namespace Tathkīr_WPF.Extensions
{
    public static class LocalizationExtensions
    {
        public static string ToInvariantLanguage(this string localizedLanguage)
        {
            var stringProperties = typeof(Strings)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(string));

            foreach (var prop in stringProperties)
            {
                var value = prop.GetValue(null) as string;

                if (string.Equals(value, localizedLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    return prop.Name;
                }
            }

            // Fallback if not found
            return localizedLanguage;
        }

        public static string ToLocalizedLanguage(this string invariantLanguage)
        {
            var property = typeof(Strings).GetProperty(
                invariantLanguage,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (property != null && property.PropertyType == typeof(string))
            {
                return property.GetValue(null) as string ?? invariantLanguage;
            }

            return invariantLanguage;
        }

    }
}

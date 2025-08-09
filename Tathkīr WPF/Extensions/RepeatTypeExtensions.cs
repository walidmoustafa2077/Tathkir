using Tathkīr_WPF.Enums;

namespace Tathkīr_WPF.Extensions
{
    public static class RepeatTypeExtensions
    {
        public static string ToLocalized(this RepeatType type)
        {
            return type switch
            {
                RepeatType.None => Strings.None,
                RepeatType.Every1Hour => $"{Strings.Every} 1 {Strings.Hour}",
                RepeatType.Every2Hours => $"{Strings.Every} 2 {Strings.Hour}",
                RepeatType.Every3Hours => $"{Strings.Every} 3 {Strings.Hour}",
                RepeatType.ThreeTimesDaily => $"3 {Strings.Times_Per_Day}",
                _ => type.ToString()
            };
        }
    }

}

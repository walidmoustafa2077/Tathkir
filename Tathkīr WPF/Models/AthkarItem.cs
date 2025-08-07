using System.Text.Json.Serialization;
using Tathkīr_WPF.Enums;

namespace Tathkīr_WPF.Models
{
    public class AthkarItem
    {
        public string Name { get; set; } = string.Empty;
        public RepeatOption SelectedRepeatOption { get; set; } = new RepeatOption { Type = RepeatType.None };
        public DateTime LastTriggered { get; set; } = DateTime.MinValue;

        // For 3 times per day
        public int TimesTriggeredToday { get; set; } = 0;
        public DateTime LastResetDate { get; set; } = DateTime.MinValue;

        [JsonIgnore]
        public List<RepeatOption> RepeatOptions { get; } = new()
        {
            new RepeatOption { Type = RepeatType.None },
            new RepeatOption { Type = RepeatType.Every1Hour },
            new RepeatOption { Type = RepeatType.Every2Hours },
            new RepeatOption { Type = RepeatType.Every3Hours },
            new RepeatOption { Type = RepeatType.ThreeTimesDaily }
        };

        public AthkarItem Clone()
        {
            return new AthkarItem
            {
                Name = this.Name,
                SelectedRepeatOption = this.SelectedRepeatOption,
                LastTriggered = this.LastTriggered,
                TimesTriggeredToday = this.TimesTriggeredToday,
                LastResetDate = this.LastResetDate
            };
        }

    }

}

using Tathkīr_WPF.Enums;
using Tathkīr_WPF.Extensions;
using Tathkīr_WPF.Models;
using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class ThikrReminderService : IThikrReminderService
    {
        private readonly IClock _clock;
        private readonly INotificationService _notifier;
        private readonly IAudioService _audioService;
        private readonly Random _random = new();

        private int _tickCounter = 0;

        public ThikrReminderService(IClock clock, INotificationService notifier, IAudioService audioService)
        {
            _clock = clock;
            _notifier = notifier;
            _audioService = audioService;
        }

        public void CheckThikrReminder()
        {
            _tickCounter++;
            if (_tickCounter < _random.Next(300, 600))
                return;

            _tickCounter = 0;

            var list = SettingsService.AppSettings.AppConfig.AthkarConfigSettings?.AthkarList;
            if (list == null || list.Count == 0)
                return;

            var now = _clock.Now;

            foreach (var thikr in list)
            {
                var shouldTrigger = thikr.SelectedRepeatOption.Type switch
                {
                    RepeatType.Every1Hour => now - thikr.LastTriggered >= TimeSpan.FromHours(1),
                    RepeatType.Every2Hours => now - thikr.LastTriggered >= TimeSpan.FromHours(2),
                    RepeatType.Every3Hours => now - thikr.LastTriggered >= TimeSpan.FromHours(3),
                    RepeatType.ThreeTimesDaily => thikr.LastResetDate.Date < now.Date && Reset(thikr, now)
                           || thikr.TimesTriggeredToday < 3 && now - thikr.LastTriggered >= TimeSpan.FromHours(3),
                    _ => false
                };

                if (shouldTrigger)
                {
                    thikr.LastTriggered = now + TimeSpan.FromMinutes(_random.Next(10, 31));
                    var path = _audioService.GetThikrAudioPath(thikr.Name.ToInvariantLanguage());
                    _notifier.NotifyThikr(thikr, path);
                    thikr.TimesTriggeredToday++;
                    SettingsService.Save(SettingsService.AppSettings);
                    // Call Onload to refresh the settings localization
                    SettingsService.AppSettings.AppConfig.Onload();
                    return;
                }
            }
        }

        private static bool Reset(AthkarItem thikr, DateTime now)
        {
            thikr.TimesTriggeredToday = 0;
            thikr.LastResetDate = now.Date;
            return true;
        }
    }

}

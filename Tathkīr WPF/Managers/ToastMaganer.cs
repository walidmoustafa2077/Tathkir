using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using Windows.UI.Notifications;

namespace Tathkīr_WPF.Managers
{
    public static class ToastMaganer
    {
        public static void Initialize()
        {
            // Register your app for toast notifications
            ToastNotificationManagerCompat.OnActivated += ToastActivated;
        }

        public static void ShowToast(string title, string message, string? soundPath = null)
        {
            var builder = new ToastContentBuilder()
                .AddText(title)
                .AddText(message);

            if (!string.IsNullOrWhiteSpace(soundPath))
            {
                // Silence toast sound so we can play our own
                builder.AddAudio(null, silent: true);
            }

            var content = builder.GetToastContent();
            var toast = new ToastNotification(content.GetXml());
            ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);

            // Play sound manually if absolute path is provided
            if (!string.IsNullOrWhiteSpace(soundPath) && File.Exists(soundPath))
            {
                Task.Run(() =>
                {
                    using var player = new System.Media.SoundPlayer(soundPath);
                    player.Play();
                });
            }
        }

        private static void ToastActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            // Optional: Handle when user clicks the toast
            // e.Argument contains any arguments you set in AddArgument()
        }
    }
}

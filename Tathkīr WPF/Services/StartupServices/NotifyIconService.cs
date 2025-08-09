using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Tathkīr_WPF.Managers;
using Tathkīr_WPF.Services.StartupServices.Interfaces;
using Application = System.Windows.Application;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class NotifyIconService : INotifyIconService
    {
        private readonly PrayerTimesManager _prayerTimesManager;
        private readonly IMainWindowService _mainWindowService;
        private readonly NotifyIcon _notifyIcon;

        public NotifyIconService(IMainWindowService mainWindowService)
        {
            _prayerTimesManager = PrayerTimesManager.Instance;
            _mainWindowService = mainWindowService;

            var exePath = Assembly.GetEntryAssembly()?.Location;
            var icon = Icon.ExtractAssociatedIcon(exePath!);

            _notifyIcon = new NotifyIcon
            {
                Icon = icon,
                Visible = false,
            };
        }

        public void Initialize()
        {
            if (_notifyIcon == null) return;

            Show();

            _notifyIcon.DoubleClick += (s, e) =>
            {
                var mainWindow = Application.Current?.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Show();
                    mainWindow.WindowState = WindowState.Normal;
                    mainWindow.Activate();
                }
                else
                    _mainWindowService.Show();
            };

            ContextMenuInitialize();

            _prayerTimesManager.CountdownUpdated += PrayerTime;
        }

        public void Show()
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = true;
        }

        public void Hide()
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = false;
        }

        public void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            }
        }

        private void ContextMenuInitialize()
        {
            var contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add("Open", null, (s, e) =>
            {
                var mainWindow = Application.Current?.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Show();
                    mainWindow.WindowState = WindowState.Normal;
                    mainWindow.Activate();
                }
            });

            // Widget toggle item
            var widgetToggleItem = new ToolStripMenuItem();
            widgetToggleItem.Text = SettingsService.AppSettings.AppConfig.ShowWidget ? "Close Widget" : "Show Widget";
            widgetToggleItem.Click += (s, e) =>
            {
                if (TathkirWidget.Instance.IsVisible)
                {
                    TathkirWidget.Instance.MainWindow_Closed(null, EventArgs.Empty);
                    TathkirWidget.Instance.Hide();
                }
                else
                {
                    TathkirWidget.Instance.InitializeWidget();
                    TathkirWidget.Instance.Show();
                }

                // Update the menu item text after toggle
                widgetToggleItem.Text = TathkirWidget.Instance.IsVisible ? "Close Widget" : "Show Widget";
            };
            contextMenu.Items.Add(widgetToggleItem);

            contextMenu.Items.Add("Restart", null, (s, e) =>
            {
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName;
                System.Diagnostics.Process.Start(exePath);
                Application.Current?.Shutdown();
            });

            contextMenu.Items.Add("Exit", null, (s, e) => Application.Current?.Shutdown());

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        private void PrayerTime()
        {
            if (_prayerTimesManager == null) return;

            Models.PrayerItem? nextPrayer = _prayerTimesManager.NextPrayer;

            var countdown = $"{(int)_prayerTimesManager.Countdown.TotalHours:D2}:{_prayerTimesManager.Countdown.Minutes:D2}:{_prayerTimesManager.Countdown.Seconds:D2}";

            if (nextPrayer != null) 
                _notifyIcon!.Text = $"{Strings.Time_Till} {nextPrayer.Name} {Strings.Is} {countdown}";
        }

    }
}

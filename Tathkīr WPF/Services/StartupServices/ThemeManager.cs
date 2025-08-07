using MaterialDesignThemes.Wpf;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class ThemeManager : Interfaces.IThemeManager
    {
        public void Apply(string themeName)
        {
            bool isDark = themeName == Strings.Dark;
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
            paletteHelper.SetTheme(theme);
        }
    }

}

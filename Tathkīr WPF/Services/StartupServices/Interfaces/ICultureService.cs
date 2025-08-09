namespace Tathkīr_WPF.Services.StartupServices.Interfaces
{
    public interface ICultureService
    {
        void SetCulture(string language);
        static bool IsRightToLeft { get; }
    }

}

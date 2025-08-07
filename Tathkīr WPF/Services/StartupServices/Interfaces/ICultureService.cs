namespace Tathkīr_WPF.Services.StartupServices.Interfaces
{
    public interface ICultureService
    {
        void SetCulture(string language);
        bool IsRightToLeft { get; }
    }

}

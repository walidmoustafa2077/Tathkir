namespace Tathkīr_WPF.Services.StartupServices.Interfaces
{
    public interface IAppInitializer
    {
        Task InitializeAsync();
        void Shutdown();
    }

}

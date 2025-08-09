using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}

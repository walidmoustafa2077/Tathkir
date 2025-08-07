using Tathkīr_WPF.Services.CoreService.Interfaces;

namespace Tathkīr_WPF.Services.CoreService
{
    public class TestClock : IClock
    {
        public DateTime Now
        {
            get
            {
                return DateTime.Now + Offset;
            }
        }

        public TimeSpan Offset { get; private set; }

        public void Advance(TimeSpan time) => Offset = time;
    }
}

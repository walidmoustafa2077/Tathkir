namespace Tathkīr_WPF.Helpers
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

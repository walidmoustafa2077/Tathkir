using Tathkīr_WPF.Enums;
using Tathkīr_WPF.Extensions;

namespace Tathkīr_WPF.Models
{
    public class RepeatOption
    {
        public RepeatType Type { get; set; }

        public string DisplayText => Type.ToLocalized();

        public override bool Equals(object? obj)
        {
            return obj is RepeatOption other && other.Type == this.Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override string ToString() => DisplayText;
    }

}

namespace TimeKit.Editor.Debugging.Windows
{
    internal static class DebugWindowUtils
    {
        internal static string FormatClockTime(double seconds)
        {
            int h = (int)(seconds / 3600);
            int m = (int)((seconds % 3600) / 60);
            double s = seconds % 60;

            if (h > 0)
                return $"{h}h {m}m {s:F1}s";
            if (m > 0)
                return $"{m}m {s:F1}s";
    
            return $"{s:F1}s";
        }
    }
}
namespace Otp.WindowsForms.Helpers
{
    public static class Formatter
    {
        public static string BytesToHumanReadableFormat(long? size)
        {
            size ??= 0;
            decimal number = (decimal)size;

            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (number >= 1024 && order < sizes.Length - 1)
            {
                order++;
                number /= 1024;
            }

            return string.Format("{0:0.##} {1}", number, sizes[order]);
        }
    }
}

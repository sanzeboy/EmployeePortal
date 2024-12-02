namespace EmployeePortal.Application.Extensions
{
    public static class StringExtension
    {
        public static string SafeSubstring(this string orig, int length)
        {
            return orig.Substring(0, orig.Length >= length ? length : orig.Length);
        }
    }
}

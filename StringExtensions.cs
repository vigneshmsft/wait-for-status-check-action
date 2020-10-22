namespace WaitForStatusCheckAction
{
    internal static class StringExtensions
    {
        public static string NullIfEmpty(this string inputString){
            return string.IsNullOrWhiteSpace(inputString) ? null : inputString;
        }
    }
}

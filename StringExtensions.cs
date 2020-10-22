using System;

namespace WaitForStatusCheckAction
{
    internal static class StringExtensions
    {
        public static string NullIfEmpty(this string inputString){
            return string.IsNullOrWhiteSpace(inputString) ? null : inputString;
        }

        public static string ThrowIfEmpty(this string inputString, string argumentName){
            return string.IsNullOrWhiteSpace(inputString) ? throw new ArgumentException($"{argumentName} cannot be empty")  : inputString;
        }
    }
}

using System;

namespace WaitForStatusCheckAction
{
    internal static class StringExtensions
    {
        public static string NullIfEmpty(this string inputString)
        {
            return string.IsNullOrWhiteSpace(inputString) ? null : inputString;
        }

        public static string ThrowIfEmpty(this string inputString, string argumentName)
        {
            return string.IsNullOrWhiteSpace(inputString) ? throw new ArgumentException($"{argumentName} cannot be empty") : inputString;
        }

        public static string[] MultilineToArray(this string inputString)
        {
            return inputString?.Replace('\'', '\0').Split(Environment.NewLine) ?? new string[0];
        }

        public static string AsCsv(this string[] stringArray)
        {
            return string.Join(", ", stringArray);
        }

        public static void WriteToConsole<T>(this T[] @array)
        {
            foreach (var @element in @array)
            {
                WriteToConsole(@element);
            }
        }

        public static void WriteToConsole<T>(this T @element)
        {
            Console.WriteLine(@element.ToString());
        }
    }
}

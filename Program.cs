using System;

namespace WaitForStatusCheckAction
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine("Hello {0}", arg);
            }
            Console.WriteLine($"::set-output name=time::{DateTime.Now}");
        }
    }
}

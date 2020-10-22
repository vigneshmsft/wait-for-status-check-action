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
            var statusCheckName = args[0];
            var repository = NullIfEmpty(args[1]) ?? Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            var sha = NullIfEmpty(args[2]) ?? Environment.GetEnvironmentVariable("GITHUB_SHA");
            
            WaitForStatusCheckAction(statusCheckName, repository, sha);
            Console.WriteLine($"::set-output name=time::{DateTime.Now}");
        }

        private static void WaitForStatusCheckAction(string statusCheckName, string repository, string commitSha)
        {
            Console.WriteLine($"{nameof(statusCheckName)}: {statusCheckName}");
            Console.WriteLine($"{nameof(repository)}: {repository}");
            Console.WriteLine($"{nameof(commitSha)}: {commitSha}");
        }

        private static string NullIfEmpty(string inputString){
            return string.IsNullOrWhiteSpace(inputString) ? null : inputString;
        }
    }
}

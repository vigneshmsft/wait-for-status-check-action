using System;

namespace WaitForStatusCheckAction
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine($"{arg}");
            }

            var statusCheckName = args[0];
            var repository = args[1].NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            var sha = args[2].NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_SHA");
            var githubApiUrl = Environment.GetEnvironmentVariable("GITHUB_API_URL");

            WaitForStatusCheckAction(statusCheckName, repository, sha, githubApiUrl);
            Console.WriteLine($"::set-output name=time::{DateTime.Now}");
        }

        private static void WaitForStatusCheckAction(string statusCheckName, string repository, string commitSha, string githubApiUrl)
        {
            Console.WriteLine($"{nameof(statusCheckName)}: {statusCheckName}");
            Console.WriteLine($"{nameof(repository)}: {repository}");
            Console.WriteLine($"{nameof(commitSha)}: {commitSha}");
            Console.WriteLine($"{nameof(githubApiUrl)}: {githubApiUrl}");
        }
    }
}

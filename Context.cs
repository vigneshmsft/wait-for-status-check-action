using System;

namespace WaitForStatusCheckAction
{
    internal class Context
    {
        public string Sha { get; private set; }

        public string Token { get; private set; }

        public string Repository { get; private set; }

        public string StatusCheckName {get; private set;}

        private Context(string token, string repository, string sha, string statusCheckName)
        {
            Token = token.ThrowIfEmpty(nameof(token));
            Repository = repository.NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            Sha = sha.NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_SHA");
            StatusCheckName = statusCheckName.ThrowIfEmpty(nameof(statusCheckName));
        }

        public static Context FromArgs(string[] args)
        {
            var token = args[0].ThrowIfEmpty("GITHUB_TOKEN");
            var repository = args[1].NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
            var sha = args[2].NullIfEmpty() ?? Environment.GetEnvironmentVariable("GITHUB_SHA");
            var statusCheckName = args[3].ThrowIfEmpty("status-check-name");
            return new Context(token, repository, sha, statusCheckName);
        }
    }
}

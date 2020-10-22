using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WaitForStatusCheckAction
{
    class Program
    {
        static readonly HttpClient apiClient = new HttpClient();

        static async Task Main(string[] args)
        {
            var index = 0;
            foreach (var arg in args)
            {
                Console.WriteLine($"{++index} => {arg}");
            }

            var context = Context.FromArgs(args);
            InitialiseApiClient(context);
            await WaitForStatusCheckAction(context);
            Console.WriteLine($"::set-output name=time::{DateTime.Now}");
        }

        private static async Task WaitForStatusCheckAction(Context context)
        {
            Console.WriteLine($"::debug::StatusCheckName: {context.StatusChecks.AsCsv()}");
            Console.WriteLine($"::debug::Repository: {context.Repository}");
            Console.WriteLine($"::debug::Sha: {context.Sha}");
            Console.WriteLine($"::debug::Check Interval: {context.WaitInterval.TotalSeconds}s");
            Console.WriteLine($"::debug::Timeout: {context.Timeout.TotalSeconds}s");

            Task.WaitAll(new[] { CheckCommitStatus(context) }, (int)context.Timeout.TotalMilliseconds);
            await Task.CompletedTask;
        }

        private static async Task CheckCommitStatus(Context context)
        {
            var statuses = await apiClient.GetFromJsonAsync<Status[]>($"/repos/{context.Repository}/commits/{context.Sha}/statuses");
            statuses.WriteToConsole();
            var requiredStatus = statuses.Where(s => context.StatusChecks.Contains(s.Context, StringComparer.InvariantCultureIgnoreCase));
            if (!requiredStatus.Any())
            {
                Console.WriteLine($"::debug::No commit status matching given conditions found in repo for the given sha.");
                Console.WriteLine($"Waiting for {context.WaitInterval.TotalSeconds}s before next check");
                await Task.Delay(context.WaitInterval);
                await CheckCommitStatus(context);
                return;
            }

            var statusChecksStates = new bool[context.StatusChecks.Length];
            var done = requiredStatus.All(status =>
            {
                status.WriteToConsole();
                return status.State.Equals("success");
            });

            if (!done)
            {
                await Task.Delay(context.WaitInterval);
                await CheckCommitStatus(context);
                return;
            }

            await Task.CompletedTask;
        }

        private static void InitialiseApiClient(Context context)
        {
            apiClient.BaseAddress = new Uri("https://api.github.com");
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {context.Token} ");
            apiClient.DefaultRequestHeaders.Add("User-Agent", "vigneshmsft WaitForStatusCheck v0.1.0");
        }
    }
}

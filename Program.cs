using System;
using System.Linq;
using System.Threading;
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
            Console.WriteLine($"StatusCheckName: {context.StatusChecks.AsCsv()}");
            Console.WriteLine($"Repository: {context.Repository}");
            Console.WriteLine($"Sha: {context.Sha}");
            Console.WriteLine($"Check Interval: {context.WaitInterval}");
            Console.WriteLine($"Timeout: {context.Timeout}");
            
            Task.WaitAll(new[]{CheckCommitStatus(context)}, (int)context.Timeout.TotalMilliseconds);
            await Task.CompletedTask;
        }

        private static async Task CheckCommitStatus(Context context)
        {
            var statuses = await apiClient.GetFromJsonAsync<Status[]>($"/repos/{context.Repository}/commits/{context.Sha}/statuses");
            if (!statuses.Any())
            {
                Console.WriteLine($"No commit status found in repo for the given sha.");
                await Task.Delay(context.WaitInterval);
                return;
            }
            foreach (var status in statuses)
            {
                Console.WriteLine($"{status.Context} is at state {status.State}");
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

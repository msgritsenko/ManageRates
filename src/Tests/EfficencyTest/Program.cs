using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Policies;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Efficiency.Test
{
    /// <summary>
    /// Makes grap with results of throttling in dependency of thread count.
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            int RATE = int.Parse( args.FirstOrDefault() ?? "2");
            int SECONDS = int.Parse(args.Skip(1).FirstOrDefault() ?? "10");
            int THREADPOW2 = int.Parse(args.Skip(2).FirstOrDefault() ?? "7");
            string outputPath = args.Skip(3).FirstOrDefault() ?? "../output.png";

            Console.WriteLine($"rate = {RATE}, seconds = {SECONDS}, pow = {THREADPOW2}");

            // prepare data
            var threadCount = Enumerable.Range(0, THREADPOW2).Select(t => 1.0 * t);
            var okReaponses = new List<double>();

            foreach (var threads in threadCount)
                okReaponses.Add(await CheckOkReasponses((int)Math.Pow(2, threads), RATE, SECONDS));

            // prepare graph
            var plt = new ScottPlot.Plot(800, 600);

            plt.PlotScatter(threadCount.ToArray(), okReaponses.ToArray(), lineWidth: 2);

            plt.PlotHLine(RATE * SECONDS, label: "correct count", lineStyle: LineStyle.Dot);

            plt.Axis(-1, THREADPOW2, 0, RATE * SECONDS * 2);
            plt.YLabel("'YES' answers");
            plt.XLabel("thread count 2^x");


            // save graph
            plt.SaveFig(outputPath);
        }


        public static async Task<int> CheckOkReasponses(int threadCount, int rate, int seconds)
        {
            ITimeService timeService = new TimeService();

            var options = new MemoryCacheOptions();
            IMemoryCache memoryCache = new MemoryCache(Options.Create(options));

            var policy = new KeyedTimeRatePolicy(rate, TimeSpan.FromSeconds(1));


            int responseTrue = 0;
            int responseFalse = 0;

            Action<bool> registerResult = code =>
            {
                if (code)
                    Interlocked.Increment(ref responseTrue);
                else
                    Interlocked.Increment(ref responseFalse);
            };

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

            var tasks = Enumerable.Range(0, threadCount)
                .Select(i => TestEndpoint(policy, cts.Token, registerResult, timeService, memoryCache))
                .ToList();

            await Task.WhenAll(tasks);

            return responseTrue;
        }

        private static async Task TestEndpoint(
        IKeyedManageRatePolicy policy,
        CancellationToken cancellationToken,
        Action<bool> registerResult,
        ITimeService timeService,
        IMemoryCache memoryCache)
        {
            // take a new task-thread from task factory
            await Task.Yield();

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = policy.IsPermitted("/key", timeService, memoryCache);
                registerResult(response);
            }
        }
    }
}

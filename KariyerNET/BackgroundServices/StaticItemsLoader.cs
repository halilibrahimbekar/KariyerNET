using Common.Common.Helper;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KariyerNET.BackgroundServices
{
    public class StaticItemsLoader : IHostedService, IDisposable
    {
        private Timer? _timer = null;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Re-fill data every 5 minutes

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            // could be read from database or other data source..
            var settingsJson = @"{""DefaultAddvertCount"":""2"",""RetrictedWords"":""tembel,ucuz"",""DefaultAdvertDay"":""15""}";

            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(settingsJson);

            Settings.Items = settings;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

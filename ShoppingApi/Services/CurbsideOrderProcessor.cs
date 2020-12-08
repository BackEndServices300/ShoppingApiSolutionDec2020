using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Services
{
    public class CurbsideOrderProcessor : BackgroundService
    {
        private readonly ILogger<CurbsideOrderProcessor> _logger;

        public CurbsideOrderProcessor(ILogger<CurbsideOrderProcessor> logger)
        {
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(true)
            {
                await Task.Delay(1000);
                _logger.LogInformation("Background worker doing it's thing");
                if(stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }
    }
}

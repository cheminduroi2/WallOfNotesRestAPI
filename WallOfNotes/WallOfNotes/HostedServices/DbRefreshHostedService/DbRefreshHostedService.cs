using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WallOfNotes.HostedServices.DbRefreshHostedService
{
    public class DbRefreshHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<DbRefreshHostedService> logger;
        private readonly IServiceScopeFactory scopeFactory;
        private DbRefresher dbRefresher;
        
        private Timer timer;

        public DbRefreshHostedService(ILogger<DbRefreshHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
            dbRefresher = new DbRefresher();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DB Refresh Hosted Service running.");
            dbRefresher.Initialize(scopeFactory);
            
            // run task every 24 hrs
            timer = new Timer(dbRefresher.RefreshDb, null, TimeSpan.FromHours(24), TimeSpan.FromHours(24));
            return Task.CompletedTask; ;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DB Refresh Hosted Service is stopping.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            dbRefresher.Terminate();
            timer?.Dispose();
        }
    }
}

using AgriculturalTech.API.Data;
using AgriculturalTech.API.Services.Interfaces;
using AgriculturalTech.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgriculturalTech.API.BackgroundServices
{
    public class SubscriptionsService : BackgroundService
    {
        private readonly ILogger<SubscriptionsService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromHours(6);

        public SubscriptionsService(
            ILogger<SubscriptionsService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Subscriptions Sync Service started");

            // Run immediately on startup
            await SyncSubscriptionAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);

                try
                {
                    await SyncSubscriptionAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing Subscriptions data");
                }
            }

            _logger.LogInformation("Subscriptions Sync Service stopped");
        }

        private async Task SyncSubscriptionAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var subscriptionService = scope.ServiceProvider.GetRequiredService<IUserSubscriptionRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            // Get unique user locations
            var userSubscriptions = await context.UserSubscriptions
                .Where(u => u.SubscriptionStatus == "active")
                .Select(u => new { u.CurrentPeriodEnd, u.CancelAtPeriodEnd, u.User.FcmToken })
                .Distinct()
                .ToListAsync();

            foreach (var subscription in userSubscriptions)
            {
                // send notification 7 days before subscription expires

                if (subscription.CurrentPeriodEnd <= DateTime.UtcNow.AddDays(7))
                {
                    string message = subscription.CancelAtPeriodEnd
                        ? $"Your subscription will expire after {subscription.CurrentPeriodEnd - DateTime.UtcNow} days and will not renew automatically. Please renew to continue enjoying our services."
                        : $"Your subscription will renew after {subscription.CurrentPeriodEnd - DateTime.UtcNow} days";

                    await notificationService.SendNotificationAsync(subscription.FcmToken, "Subscription status", message);
                }
            }
        }
    }
}

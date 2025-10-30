using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Checks for due reminders and sends notifications every 15 minutes
/// </summary>
public class ReminderNotificationService : BackgroundService
{
    private readonly ILogger<ReminderNotificationService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public ReminderNotificationService(
        ILogger<ReminderNotificationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reminder Notification Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessRemindersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing reminders");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("Reminder Notification Service stopped");
    }

    private async Task ProcessRemindersAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var now = DateTime.UtcNow;
        var checkUntil = now.AddMinutes(15); // Check reminders due in next 15 minutes

        var dueReminders = await context.CropReminders
            .Where(r => r.IsActive
                && !r.IsCompleted
                && !r.NotificationSent
                && r.ReminderDate >= now
                && r.ReminderDate <= checkUntil)
            .ToListAsync();

        _logger.LogInformation($"Found {dueReminders.Count} reminders to process");

        foreach (var reminder in dueReminders)
        {
            try
            {
                await notificationService.SendReminderNotificationAsync(reminder.UserId, reminder);

                reminder.NotificationSent = true;
                context.CropReminders.Update(reminder);

                _logger.LogInformation($"Sent notification for reminder {reminder.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notification for reminder {reminder.Id}");
            }
        }

        if (dueReminders.Any())
        {
            await context.SaveChangesAsync();
        }

        // Handle recurring reminders
        await HandleRecurringRemindersAsync(context);
    }

    private async Task HandleRecurringRemindersAsync(ApplicationDbContext context)
    {
        var completedRecurringReminders = await context.CropReminders
            .Where(r => r.IsActive
                && r.IsCompleted
                && r.IsRecurring
                && r.RecurringIntervalDays.HasValue)
            .ToListAsync();

        foreach (var reminder in completedRecurringReminders)
        {
            // Create next occurrence
            var nextReminder = new CropReminder
            {
                UserId = reminder.UserId,
                UserPlantId = reminder.UserPlantId,
                Title = reminder.Title,
                Description = reminder.Description,
                ReminderType = reminder.ReminderType,
                ReminderDate = reminder.ReminderDate.AddDays(reminder.RecurringIntervalDays.Value),
                IsRecurring = true,
                RecurringIntervalDays = reminder.RecurringIntervalDays,
                Priority = reminder.Priority
            };

            await context.CropReminders.AddAsync(nextReminder);

            // Deactivate old reminder
            reminder.IsActive = false;
            context.CropReminders.Update(reminder);
        }

        await context.SaveChangesAsync();
    }
}
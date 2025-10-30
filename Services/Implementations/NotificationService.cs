public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    // In real implementation, inject push notification service (Firebase, SignalR, etc.)

    public NotificationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SendReminderNotificationAsync(string userId, CropReminder reminder)
    {
        var notification = new Notification
        {
            UserId = userId,
            NotificationType = "Reminder",
            Title = $"Reminder: {reminder.Title}",
            Message = reminder.Description ?? $"It's time for: {reminder.ReminderType}",
            ActionLink = $"/plants/{reminder.UserPlantId}"
        };

        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        // TODO: Send push notification via Firebase/SignalR
    }

    public async Task SendAlertNotificationAsync(string userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            NotificationType = "Alert",
            Title = title,
            Message = message
        };

        await _unitOfWork.Notifications.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
    {
        var notifications = await _unitOfWork.Notifications
            .FindAsync(n => n.UserId == userId && !n.IsRead);

        return notifications.OrderByDescending(n => n.CreatedAt).ToList();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
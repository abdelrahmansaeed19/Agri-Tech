public interface INotificationService
{
    Task SendReminderNotificationAsync(string userId, CropReminder reminder);
    Task SendAlertNotificationAsync(string userId, string title, string message);
    Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
    Task MarkAsReadAsync(int notificationId);
}